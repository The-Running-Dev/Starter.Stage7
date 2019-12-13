.\Starter.Build\build.ps1

.\Starter.Build\run.ps1
http://localhost:8080

az acr login --name astracontainerregistry

docker tag starter.api astracontainerregistry.azurecr.io/boyankostadinov-starter.api:latest
docker tag starter.messageconsumer.console astracontainerregistry.azurecr.io/boyankostadinov-starter.consumer:latest

docker push astracontainerregistry.azurecr.io/boyankostadinov-starter.api:latest
docker push astracontainerregistry.azurecr.io/boyankostadinov-starter.consumer:latest

docker run -it --entrypoint sh starter.api
docker run -it --entrypoint sh starter.consumer

az aks get-credentials --resource-group boyankostadinov-newstartertask --name dev-astra-boyankostadinov-starter

kubectl create clusterrolebinding kubernetes-dashboard `
	-n kube-system `
	--clusterrole=cluster-admin `
	--serviceaccount=kube-system:kubernetes-dashboard

kubectl apply -f kubernetes-cluster.yml && kubectl get service starter-api --watch
kubectl delete service --all && kubectl delete deployment --all

az aks browse `
	--resource-group boyankostadinov-newstartertask `
	--name dev-astra-boyankostadinov-starter

Please add "C:\Users\boyank\.azure-kubectl" to your search PATH so the `kubectl.exe` can be found. 2 options:
    1. Run "set PATH=%PATH%;C:\Users\boyank\.azure-kubectl" or "$env:path += 'C:\Users\boyank\.azure-kubectl'" for PowerShell. This is good for the current command session.
    2. Update system PATH environment variable by following "Control Panel->System->Advanced->Environment Variables", and re-open the command window. You only need to do it once

CHRISTMAS