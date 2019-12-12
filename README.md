.\Starter.Build\build.ps1

.\Starter.Build\run.ps1

docker run -it --entrypoint sh starter.api

docker run -it --entrypoint sh starter.messageconsumer.console