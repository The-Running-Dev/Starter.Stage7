# Build file for the project
$config = @{
	ProjectDirectory   = $PSScriptRoot
	Artifacts          = 'Artifacts'
	Packages           = 'Packages'
	BuildConfiguration = 'Release'
	ApiProject         = 'Starter.API'
	ConsumerProject    = 'Starter.Consumer'
	ProjectLabel       = "com.microsoft.visual-studio.project-name=`$projectName"
}

Properties `
{
	$artifacts = Join-Path $config.ProjectDirectory $config.Artifacts
	$packagesDirectory = Join-Path $config.ProjectDirectory $config.Packages
	$apiProjectFile = Join-Path (Join-Path $config.ProjectDirectory $config.ApiProject) "$($config.ApiProject).csproj"
	$apiProjectArtifacts = Join-Path (Join-Path $config.ProjectDirectory $config.Artifacts) $config.ApiProject
	$consumerProjectFile = Join-Path (Join-Path $config.ProjectDirectory $config.ConsumerProject) "$($config.ConsumerProject).csproj"
	$consumerProjectArtifacts = Join-Path (Join-Path $config.ProjectDirectory $config.Artifacts) $config.consumerProject

	$basePath = '.'
	$version = 0
	$branch = ''
	$publish = $true
	$release = $true
}

FormatTaskName {
	param($taskName)
	Write-Output "
========================================
$($taskName -csplit '(?=[A-Z])' -ne '' -join ' ')
========================================
"
}

Task Default `
	-Depends Build, RunTests, Publish, Clean

Task Test `
	-Depends RunTests, Clean

Task Publish `
	-Depends PublishArtifacts, CreateContainers

Task Build `
	-Depends Setup `
{
	exec {
		& dotnet build `
			$config.ProjectDirectory `
			--configuration $config.BuildConfiguration `
			--packages $packagesDirectory
	}
}

Task RunTests  `
	-Depends Build `
{
	exec {
		& dotnet test `
			$config.ProjectDirectory `
			--configuration $config.BuildConfiguration `
			--no-build
	}
}

# Publishes but only if Publish is set to true
Task PublishArtifacts  `
	-Depends Build `
	-PreCondition { $script:Publish } `
{
	Write-Output 'Publishing the API...'
	exec {
		& dotnet publish $apiProjectFile `
			--configuration $config.BuildConfiguration `
			--no-restore `
			--output $apiProjectArtifacts
	}

	Write-Output 'Publishing the Consumer...'
	exec {
		& dotnet publish $consumerProjectFile `
			--configuration $config.BuildConfiguration `
			--no-restore `
			--output $consumerProjectArtifacts
	}
}

Task CreateContainers `
	-Depends PublishArtifacts `
{
	Get-ChildItem $config.ProjectDirectory `
		-Recurs -File Dockerfile | ForEach-Object {
		$dockerFile = $_.FullName
		$projectName = (Split-Path $_.DirectoryName -Leaf)
		$projectTag = $projectName.ToLower()
		$projectLabel = $ExecutionContext.InvokeCommand.ExpandString($config.ProjectLabel)
		$context = Join-Path $artifacts $projectName

		Write-Output "
Project: $projectName
Context: $context
Dockerfile: $dockerFile
Label: $projectLabel
Tag: $projectTag
"

		Write-Output "Building $projectName Container..."
		exec {
			& docker build -f $dockerFile `
				--force-rm `
				--rm `
				-q `
				-t $projectTag `
				--label $projectLabel `
				$context
		}
	}
}

Task Clean `
{
	exec {
		& dotnet clean `
			$config.ProjectDirectory `
			-v q --nologo
	}

	# Delete the artifacts
	Remove-Item $artifacts `
		-Recurse -Force -ErrorAction SilentlyContinue | Out-Null

	# Delete all the 'bin' directories
	Get-ChildItem $config.ProjectDirectory 'bin' `
		-Recurse | Remove-Item -Recurse -Force `
		-ErrorAction SilentlyContinue | Out-Null
}

Task Setup `
{
	$script:ScriptDirectory = (Get-Item $psake.build_script_file).Directory.FullName

	# Set the Version to the one passed in, or to today's date
	$script:Version = @{$true = [DateTime]::Now.ToString("yyyy.MM.dd"); $false = $version }["" -Match $version]

	# Set the BuildId to the one passed in, or empty string
	$script:BuildId = @{$true = $script:Config.BuildId; $false = $buildId }['' -Match $buildId]

	# Release mode is 'true' unless explicitly set to 'false'
	$script:Release = @{$true = $true; $false = $false }['1,true,yes' -Match $release]

	# Publish is 'true' unless explicitly set to 'false'
	$script:Publish = @{$true = $true; $false = $false }['1,true,yes' -Match $publish]

	#Set-Location $config.ProjectDirectory
}