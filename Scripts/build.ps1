param(
	[switch] $DockerOnly = $false
)

$buildDirectory = (Split-Path $PSScriptRoot -Parent)
$buildArtifactStagingDirectory = (Join-Path $BuildDirectory 'Artifacts')
$buildConfiguration = 'Release'

$apiProject = 'Starter.API'
$apiProjectFile = Join-Path (Join-Path $BuildDirectory $apiProject) "$apiProject.csproj"
$apiProjectArtifacts = Join-Path $BuildArtifactStagingDirectory $apiProject

$consumerProject = 'Starter.Consumer'
$consumerProjectFile = Join-Path (Join-Path $buildDirectory $consumerProject) "$consumerProject.csproj"
$consumerProjectArtifacts = Join-Path $BuildArtifactStagingDirectory $consumerProject

$vsLabel = "com.microsoft.created-by=visual-studio"
$projectLabel = "com.microsoft.visual-studio.project-name=`$projectName"

if (-not $DockerOnly) {
	Write-Output 'Building...'
	& dotnet build

	Write-Output 'Running Tests...'
	& dotnet test

	Write-Output 'Publishing API...'
	& dotnet publish $apiProjectFile `
		--configuration $BuildConfiguration `
		--output $apiProjectArtifacts

	Write-Output 'Publishing Consumer...'
	& dotnet publish $consumerProjectFile `
		--configuration $BuildConfiguration `
		--output $consumerProjectArtifacts
}

Get-ChildItem -Recurs -File Dockerfile | ForEach-Object {
	$dockerFile = $_.FullName
	$projectName = (Split-Path $_.DirectoryName -Leaf)
	$projectTag = $projectName.ToLower()
	$projectLabel = $ExecutionContext.InvokeCommand.ExpandString($projectLabel)
	$context = Join-Path $BuildArtifactStagingDirectory $projectName

	Write-Output "
Project: $projectName
Context: $context
Dockerfile: $dockerFile
Label: $projectLabel
Tag: $projectTag
"

	Write-Output "Building $projectName Container..."
	& docker build -f $dockerFile `
		--force-rm `
		--rm -q `
		-t $projectTag `
		--label $vsLabel `
		--label $projectLabel `
		$context
}