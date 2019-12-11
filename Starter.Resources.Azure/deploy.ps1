#Requires -Version 3.0
#Requires -Module Az.Resources
#Requires -Module Az.Storage

Param(
    [string] [Parameter(Mandatory)] $Location,
    [string] $Artifacts = (Join-Path $PSScriptRoot 'artifacts'),
    [string] $ResourceGroup = (Split-Path $Artifacts -Leaf),
    [switch] $UploadArtifacts,
    [string] $StorageAccountName,
    [string] $StorageContainerName = $resourceGroup.ToLowerInvariant() + '-artifacts',
    [string] $TemplateFile,
    [string] $TemplateParametersFile,
    [string] $DSCSourceFolder = (Join-Path $artifacts 'dsc'),
    [switch] $BuildDscPackage,
    [switch] $ValidateOnly,
    [string] $DebugOptions = "None"
)

<#
.\deploy.ps1 -location westeurope -templateFile .\app-service.json
.\deploy.ps1 -location westeurope -templateFile .\service-bus.json
.\deploy.ps1 -location westeurope -templateFile .\storage-account.json
#>

try {
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.AddUserAgent("AzQuickStarts-$UI$($host.name)".replace(" ", "_"), "1.0")
}
catch { }

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 3

function Format-ValidationOutput {
    param ($validationOutput, [int] $depth = 0)
    Set-StrictMode -Off
    
    return @($validationOutput | `
        Where-Object { $_ -ne $null } | `
        ForEach-Object { @('  ' * $depth + ': ' + $_.Message) + @(Format-ValidationOutput @($_.details) ($depth + 1)) })
}

function Invoke-Template {
    $optionalParameters = New-Object -TypeName Hashtable
    $templateArgs = New-Object -TypeName Hashtable
    $artifacts = ($artifacts.TrimEnd('/')).TrimEnd('\')
    $deploymentName = "$(Split-Path $templateFile -LeafBase)-$(Get-Date -f 'yyyy.MM.dd-HH.mm')"

    # No parameter file specified, set parameter file to 'template.parameters.json''
    if (-not (Test-Path $templateParametersFile)) {
        $templateParametersFile = Join-Path $(Split-Path $templateFile -Parent) "$(Split-Path $templateFile -LeafBase).parameters.json"
    }

    if (!$validateOnly) {
        $optionalParameters.Add('DeploymentDebugLogLevel', $debugOptions)
    }

    $templateFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $templateFile))
    $templateParametersFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $templateParametersFile))
    $templateJSON = Get-Content $templateFile -Raw | ConvertFrom-Json
    $templateSchema = $templateJson | Select-Object -expand '$schema' -ErrorAction Ignore

    if ($templateSchema -like '*subscriptionDeploymentTemplate.json*') {
        $deploymentScope = "Subscription"
    }
    else {
        $deploymentScope = "ResourceGroup"
    }

    Write-Output "$deploymentName $deploymentScope Deployment..."

    $artifactsLocationParameter = $templateJson | `
        Select-Object -expand 'parameters' -ErrorAction Ignore | `
        Select-Object -Expand '_artifactsLocation' -ErrorAction Ignore

    # If the switch is set or the standard parameter is present in the template, upload all artifacts
    if ($uploadArtifacts -Or $artifactsLocationParameter -ne $null) {
        # Convert relative paths to absolute paths
        $artifacts = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $artifacts))
        $dscSourceFolder = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $dscSourceFolder))

        # Parse the parameter file and update the values of artifacts location and artifacts location SAS token if they are present
        $jsonParameters = Get-Content $templateParametersFile -Raw | ConvertFrom-Json
    
        if (($jsonParameters | Get-Member -Type NoteProperty 'parameters') -ne $null) {
            $jsonParameters = $jsonParameters.parameters
        }

        $artifactsLocationName = '_artifactsLocation'
        $artifactsLocationSasTokenName = '_artifactsLocationSasToken'
        $optionalParameters[$artifactsLocationName] = $jsonParameters | `
            Select-Object -Expand $ArtifactsLocationName -ErrorAction Ignore | `
            Select-Object -Expand 'value' `
            -ErrorAction Ignore
        $optionalParameters[$artifactsLocationSasTokenName] = $jsonParameters | `
            Select-Object -Expand $artifactsLocationSasTokenName -ErrorAction Ignore | `
            Select-Object -Expand 'value' -ErrorAction Ignore

        # Create DSC configuration archive
        if ((Test-Path $dscSourceFolder) -and ($buildDscPackage)) {
            $dscSourceFilePaths = @(Get-ChildItem $dscSourceFolder -File -Filter '*.ps1' | `
                ForEach-Object -Process { $_.FullName })
        
            foreach ($dscSourceFilePath in $dscSourceFilePaths) {
                $dscArchiveFilePath = $dscSourceFilePath.Substring(0, $dscSourceFilePath.Length - 4) + '.zip'

                Publish-AzVMDscConfiguration $dscSourceFilePath `
                    -OutputArchivePath $dscArchiveFilePath `
                    -Force -Verbose
            }
        }

        # Create a storage account name if none was provided
        if ($storageAccountName -eq '') {
            $storageAccountName = 'stage' + ((Get-AzContext).Subscription.Id).Replace('-', '').substring(0, 19)
        }

        $storageAccount = (Get-AzStorageAccount | Where-Object { $_.storageAccountName -eq $storageAccountName })

        # Create the storage account if it doesn't already exist
        if ($storageAccount -eq $null) {
            $storageResourceGroupName = 'ARM_Deploy_Staging'

            New-AzResourceGroup `
                -Location "$location" `
                -Name $storageResourceGroupName `
                -Force
        
            $storageAccount = New-AzStorageAccount `
                -StorageAccountName $storageAccountName `
                -Type 'Standard_LRS' `
                -ResourceGroupName $storageResourceGroupName `
                -Location "$location"
        }

        if ($storageContainerName.length -gt 63) {
            $storageContainerName = $storageContainerName.Substring(0, 63)
        }

        $artifactStagingLocation = $storageAccount.Context.BlobEndPoint + $storageContainerName + "/"   

        # Generate the value for artifacts location if it is not provided in the parameter file
        if ($optionalParameters[$artifactsLocationName] -eq $null) {
            # if the defaultValue for _artifactsLocation is using the template location,
            # use the defaultValue, otherwise set it to the staging location
            $defaultValue = $artifactsLocationParameter | `
                Select-Object -Expand 'defaultValue' -ErrorAction Ignore
        
            if ($defaultValue -like '*deployment().properties.templateLink.uri*') {
                $optionalParameters.Remove($artifactsLocationName)
            }
            else {
                $optionalParameters[$artifactsLocationName] = $artifactStagingLocation
            }
        } 

        # Copy files from the local storage staging location to the storage account container
        New-AzStorageContainer `
            -Name $storageContainerName `
            -Context $storageAccount.Context `
            -ErrorAction SilentlyContinue *>&1

        $artifactFilePaths = Get-ChildItem $artifacts -Recurse -File | `
            ForEach-Object -Process { $_.FullName }
    
        foreach ($sourcePath in $artifactFilePaths) {
            if ($sourcePath -like "$dscSourceFolder*" -and $sourcePath -like "*.zip" -or !($sourcePath -like "$dscSourceFolder*")) {
                # When using DSC, just copy the DSC archive, not all the modules and source files
                $blobName = ($SourcePath -ireplace [regex]::Escape($artifacts), "").TrimStart("/").TrimStart("\")
            
                Set-AzStorageBlobContent `
                    -File $sourcePath `
                    -Blob $blobName `
                    -Container $storageContainerName `
                    -Context $storageAccount.Context `
                    -Force
            }
        }

        # Generate a 4 hour SAS token for the artifacts location if one was not provided in the parameters file
        if ($optionalParameters[$artifactsLocationSasTokenName] -eq $null) {
            $optionalParameters[$artifactsLocationSasTokenName] = (
                New-AzStorageContainerSASToken `
                    -Container $storageContainerName `
                    -Context $storageAccount.Context `
                    -Permission r `
                    -ExpiryTime (Get-Date).AddHours(4))
        }

        $templateArgs.Add('TemplateUri',
            $artifactStagingLocation +
            (Get-ChildItem $templateFile).Name +
            $optionalParameters[$artifactsLocationSasTokenName]
        )

        $optionalParameters[$artifactsLocationSasTokenName] = ConvertTo-SecureString `
            $optionalParameters[$artifactsLocationSasTokenName] `
            -AsPlainText `
            -Force
    }
    else {
        $templateArgs.Add('TemplateFile', $templateFile)
    }

    $templateArgs.Add('TemplateParameterFile', $templateParametersFile)

    Write-Output ($templateArgs | Out-String)
    Write-Output ($optionalParameters | Out-String)

    # Create the resource group only when it doesn't already exist
    # and only in RG scoped deployments
    if ($deploymentScope -eq "ResourceGroup") {
        if ((Get-AzResourceGroup `
                -Name $resourceGroup `
                -Location $Location `
                -Verbose `
                -ErrorAction SilentlyContinue) -eq $null) {

            New-AzResourceGroup `
                -Name $resourceGroup `
                -Location $location `
                -Verbose `
                -Force `
                -ErrorAction Stop
        }
    }

    if ($validateOnly) {
        if ($deploymentScope -eq "Subscription") {
            # Subscription scoped deployment
            $errorMessages = Format-ValidationOutput (
                Test-AzDeployment `
                    -Location $location `
                    @TemplateArgs `
                    @OptionalParameters
            )
        }
        else {
            # ResourceGroup deployment 
            $errorMessages = Format-ValidationOutput (
                Test-AzResourceGroupDeployment `
                    -ResourceGroupName $resourceGroup `
                    @templateArgs `
                    @optionalParameters
            )
        }

        if ($errorMessages) {
            Write-Output '', 'Validation Errors: ', @($errorMessages), '', 'Template is Invalid...'
        }
        else {
            Write-Output '', 'Template is Valid...'
        }
    }
    else {
        # Switch to Continue" so multiple errors can be formatted and output
        $ErrorActionPreference = 'Continue'
    
        if ($deploymentScope -eq "Subscription") {
            New-AzDeployment `
                -Name $DeploymentName `
                -Location $Location `
                @templateArgs `
                @optionalParameters `
                -Verbose `
                -ErrorVariable ErrorMessages
        }
        else {
            New-AzResourceGroupDeployment `
                -Name $deploymentName `
                -ResourceGroupName $resourceGroup `
                @templateArgs `
                @optionalParameters `
                -Force -Verbose `
                -ErrorVariable ErrorMessages
        }

        $ErrorActionPreference = 'Stop' 

        if ($errorMessages) {
            Write-Output '', 'Errors:', '', @(@($errorMessages) | ForEach-Object { $_.Exception.Message })
            Write-Error "Deployment Failed..."
        }
    }
}

if (-not $templateFile) {
    Get-ChildItem $PSScriptRoot *.json | ? Name -notmatch 'parameters' | ForEach-Object {
        $templateFile = $_.FullName

        Invoke-Template
    }
}
else {
    Invoke-Template
}