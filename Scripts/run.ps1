param(
	[switch] $AsDaemon = $false,
	[switch] $FromAzure = $false
)

$projectDir = Split-Path $PSScriptRoot -Parent
$manifestsDir = Join-Path $projectDir 'Manifests' -Resolve
$command = 'docker-compose'
$environment = @{$true = 'azure'; $false = 'local';}[$fromAzure -eq $true]
$environmentVariablesFile = "docker-compose.env.$environment"

$fileSwitch = '-f'
$recreateSwitch = '--force-recreate'
$services = 'starter.api'
$composeFile = Join-Path $manifestsDir 'docker-compose.yml' -Resolve
$upArgument = 'up'
$detachArgument = @{$true = '-d'; $false = '';}[$asDaemon -eq $true]
$arguments = @($fileSwitch, $composeFile, $upArgument, $detachArgument)
$dockerImages = @()

# Read the contents of the appropriate environment file, ignoring empty lines
Get-Content (Join-Path $manifestsDir $environmentVariablesFile) | `
	Where-Object { $_.Trim() -ne '' } | ForEach-Object {
	# Get the variable and its value by splitting on the plus
	$variable = $_ -Split '=' | Select-Object -First 1
	$value = $_ -Split '=' | Select-Object -Last 1

	# Create the variable as an environment variable
	Set-Item "env:$variable" $value

	# Keep track of the images defined in the environment file
	if ($variable -match '_image') {
		$dockerImages += $value
	}
}

# For non local environments
if ($environment -ne 'local') {
	# Make sure we have the latest docker images
	$dockerImages | ForEach-Object {
		docker pull $_
	}
}

& $command $arguments