param(
	[switch] $AsDaemon = $false
)

$command = 'docker-compose'

$fileSwitch = '-f'
$composeFile = (Join-Path $PSScriptRoot 'docker-compose.yaml' -Resolve)
$upArgument = 'up'
$detachArgument = @{$true = '-d'; $false = '';}[$AsDaemon -eq $true]

$arguments = @($fileSwitch, $composeFile, $upArgument, $detachArgument)

& $command $arguments