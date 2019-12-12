param(
	[switch] $DockerOnly = $false
)

$BuildDirectory = (Split-Path $PSScriptRoot -Parent)
$BuildArtifactStagingDirectory = (Join-Path $BuildDirectory 'Artifacts')
$BuildConfiguration = 'Release'

$apiProject = 'Starter.API'
$apiProjectFile = Join-Path (Join-Path $BuildDirectory $apiProject) "$apiProject.csproj"
$apiProjectArtifacts = Join-Path $BuildArtifactStagingDirectory $apiProject

$messageConsumerProject = 'Starter.MessageConsumer.Console'
$messageConsumerProjectFile = Join-Path (Join-Path $BuildDirectory $messageConsumerProject) "$messageConsumerProject.csproj"
$messageConsumerProjectArtifacts = Join-Path $BuildArtifactStagingDirectory $messageConsumerProject

$vsLabel = "com.microsoft.created-by=visual-studio"
$projectLabel = "com.microsoft.visual-studio.project-name=`$projectName"

if (-not $DockerOnly) {
	Write-Output 'Converting to Bits...'
	& dotnet build

	Write-Output 'Common, Common, Pass Already...'
	& dotnet test

	Write-Output 'Publishing API...'
	& dotnet publish $apiProjectFile `
		--configuration $BuildConfiguration `
		--output $apiProjectArtifacts

	Write-Output 'Publishing Message Consumer...'
	& dotnet publish $messageConsumerProjectFile `
		--configuration $BuildConfiguration `
		--output $messageConsumerProjectArtifacts
}

Get-ChildItem -Recurs -File Dockerfile | ForEach-Object {
	$dockerFile = $_.FullName
	$projectName = (Split-Path $_.DirectoryName -Leaf)
	$projectTag = $projectName.ToLower()
	$projectLabel = $ExecutionContext.InvokeCommand.ExpandString($projectLabel)
	$artificats = Join-Path $BuildArtifactStagingDirectory $projectName

	Write-Output "
Project: $projectName
Artifacts: $artificats
Dockerfile: $dockerFile
Label: $projectLabel
Tag: $projectTag
"

	Write-Output "Building $projectName Container..."
	& docker build -f $dockerFile `
		--force-rm `
		-t $projectTag `
		--label $vsLabel `
		--label $projectLabel `
		--no-cache `
		$artificats
}