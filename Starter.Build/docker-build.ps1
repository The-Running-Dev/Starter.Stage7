$vsLabel = "com.microsoft.created-by=visual-studio"
$projectLabel = "com.microsoft.visual-studio.project-name=`$projectName"

Get-ChildItem -Recurs -File Dockerfile | ForEach-Object {
	$dockerFile = $_.FullName
	$projectName = (Split-Path $_.DirectoryName -Leaf)
	$projectTag = $projectName.ToLower()
	$projectLabel = $ExecutionContext.InvokeCommand.ExpandString($projectLabel)

	Write-Output "
Dockerfile: $dockerFile
Project: $projectName
Tag: $projectTag
Label: $projectLabel
"

	docker build -f $dockerFile `
		--force-rm `
		-t $projectTag `
		--label $vsLabel `
		--label $projectLabel `
		.
}