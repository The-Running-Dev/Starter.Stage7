Param(
    [string] [Parameter(Mandatory=$true)] $resourceGroupLocation,
    [string] [Parameter(Mandatory=$true)] $resourceGroupName,
    [switch] $uploadArtifacts,
    [string] $storageAccountName,
    [string] $storageContainerName = $ResourceGroupName.ToLowerInvariant() + '-artifacts',
    [string] $templateFile = 'azuredeploy.json',
    [string] $templateParametersFile = 'azuredeploy.parameters.json',
    [string] $artifactStagingDirectory = '.',
    [string] $dscSourceFolder = 'DSC',
    [switch] $validateOnly
)

try {
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.AddUserAgent("VSAzureTools-$UI$($host.name)".replace(' ','_'), '3.0.0')
} catch { }

$ErrorActionPreference = 'Stop'

function Format-ValidationOutput {
    param ($ValidationOutput, [int] $Depth = 0)
    Set-StrictMode -Off
    
    return @($ValidationOutput | `
        Where-Object { $_ -ne $null } | `
        ForEach-Object { @('  ' * $Depth + ': ' + $_.Message) + @(Format-ValidationOutput @($_.Details) ($Depth + 1)) })
}

$optionalParameters = New-Object -TypeName Hashtable
$templateFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $TemplateFile))
$templateParametersFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $TemplateParametersFile))

if ($uploadArtifacts) {
    # Convert relative paths to absolute paths if needed
    $artifactStagingDirectory = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $ArtifactStagingDirectory))
    $dscSourceFolder = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $DSCSourceFolder))

    # Parse the parameter file and update the values of artifacts location and artifacts location SAS token if they are present
    $jsonParameters = Get-Content $TemplateParametersFile -Raw | ConvertFrom-Json
    
    if (($JsonParameters | Get-Member -Type NoteProperty 'parameters') -ne $null) {
        $JsonParameters = $JsonParameters.parameters
    }

    $artifactsLocationName = '_artifactsLocation'
    $artifactsLocationSasTokenName = '_artifactsLocationSasToken'
    $optionalParameters[$ArtifactsLocationName] = $JsonParameters | `
        Select -Expand $ArtifactsLocationName -ErrorAction Ignore | `
        Select -Expand 'value' -ErrorAction Ignore
    $optionalParameters[$ArtifactsLocationSasTokenName] = $JsonParameters | `
        Select -Expand $ArtifactsLocationSasTokenName -ErrorAction Ignore | `
        Select -Expand 'value' -ErrorAction Ignore

    # Create DSC configuration archive
    if (Test-Path $DSCSourceFolder) {
        $dscSourceFilePaths = @(Get-ChildItem $DSCSourceFolder -File -Filter '*.ps1' | ForEach-Object -Process {$_.FullName})
        
        foreach ($DSCSourceFilePath in $DSCSourceFilePaths) {
            $DSCArchiveFilePath = $DSCSourceFilePath.Substring(0, $DSCSourceFilePath.Length - 4) + '.zip'

            Publish-AzVMDscConfiguration $DSCSourceFilePath -OutputArchivePath $DSCArchiveFilePath -Force -Verbose
        }
    }

    # Create a storage account name if none was provided
    if ($StorageAccountName -eq '') {
        $StorageAccountName = 'stage' + ((Get-AzureContext).Subscription.SubscriptionId).Replace('-', '').substring(0, 19)
    }

    $storageAccount = (Get-AzureStorageAccount | Where-Object { $_.StorageAccountName -eq $StorageAccountName })

    # Create the storage account if it doesn't already exist
    if ($StorageAccount -eq $null) {
        $StorageResourceGroupName = 'ARM_Deploy_Staging'

        New-AzureResourceGroup -Location "$ResourceGroupLocation" -Name $StorageResourceGroupName -Force
        
        $StorageAccount = New-AzureStorageAccount -StorageAccountName $StorageAccountName `
            -Type 'Standard_LRS' `
            -ResourceGroupName $StorageResourceGroupName `
            -Location "$ResourceGroupLocation"
    }

    # Generate the value for artifacts location if it is not provided in the parameter file
    if ($OptionalParameters[$ArtifactsLocationName] -eq $null) {
        $OptionalParameters[$ArtifactsLocationName] = $StorageAccount.Context.BlobEndPoint + $StorageContainerName
    }

    # Copy files from the local storage staging location to the storage account container
    New-AzureStorageContainer -Name $StorageContainerName -Context $StorageAccount.Context -ErrorAction SilentlyContinue *>&1

    $ArtifactFilePaths = Get-ChildItem $ArtifactStagingDirectory -Recurse -File | ForEach-Object -Process {$_.FullName}
    
    foreach ($SourcePath in $ArtifactFilePaths) {
        Set-AzureStorageBlobContent -File $SourcePath -Blob $SourcePath.Substring($ArtifactStagingDirectory.length + 1) `
            -Container $StorageContainerName -Context $StorageAccount.Context -Force
    }

    # Generate a 4 hour SAS token for the artifacts location if one was not provided in the parameters file
    if ($optionalParameters[$artifactsLocationSasTokenName] -eq $null) {
        $optionalParameters[$artifactsLocationSasTokenName] = ConvertTo-SecureString -AsPlainText -Force `
            (New-AzureStorageContainerSASToken `
                -Container $StorageContainerName `
                -Context $StorageAccount.Context `
                -Permission r `
                -ExpiryTime (Get-Date).AddHours(4))
    }
}

# Create the resource group only when it doesn't already exist
if ((Get-AzResourceGroup `
    -Name $ResourceGroupName `
    -Location $ResourceGroupLocation `
    -Verbose -ErrorAction SilentlyContinue) -eq $null) {

    New-AzResourceGroup `
        -Name $ResourceGroupName `
        -Location $ResourceGroupLocation `
        -Verbose `
        -Force -ErrorAction Stop
}

if ($validateOnly) {
    $errorMessages = Format-ValidationOutput (
        Test-AzResourceGroupDeployment `
        -ResourceGroupName $resourceGroupName `
        -TemplateFile $templateFile `
        -TemplateParameterFile $templateParametersFile `
        @optionalParameters
    )

    if ($errorMessages) {
        Write-Output '', 'Validation Returned Errors:', @($ErrorMessages), '', 'Template is Invalid...'
    }
    else {
        Write-Output '', 'Template is Valid...'
    }
}
else {
    New-AzResourceGroupDeployment `
        -Name ((Get-ChildItem $templateFile).BaseName + '-' + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')) `
        -ResourceGroupName $resourceGroupName `
        -TemplateFile $templateFile `
        -TemplateParameterFile $templateParametersFile `
        @OptionalParameters `
        -Force -Verbose `
        -ErrorVariable ErrorMessages

    if ($ErrorMessages) {
        Write-Output '', 'Deployment Errors:', @(@($ErrorMessages) | `
            ForEach-Object { $_.Exception.Message.TrimEnd("`r`n") })
    }
}