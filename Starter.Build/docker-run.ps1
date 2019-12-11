$composeFile = Join-Path $PSScriptRoot 'docker-compose.yaml' -Resolve

& docker-compose -f $composeFile up