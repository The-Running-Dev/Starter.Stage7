## Stage 7 - Azure DevOps and ARM templates

### Learning
- Build and Deploy: https://azure.microsoft.com/en-us/services/devops
- ARM Templates: https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-quickstart-create-templates-use-the-portal
- ARM Templates: http://techgenix.com/visual-studio-team-services-arm-templates/
- The section titled "Building, Deploying, and DevOps with VSTS" is really helpfull: https://app.pluralsight.com/library/courses/getting-started-visual-studio-team-services-2018/table-of-contents

### Tasks
- Repo: Create a source code repository in VSTS/Devops to hold your code.
- Build: Create a build which also runs your unit tests
- Release: Create a SEPARATE release pipeline which deploys the artifacts created from the build to your resource group.
- ARM Templates: Download the ARM template for your resource group. Understand how the combination of values tells azure how to create your resource group.
- Deploy ARM Templates: Use VSTS to deploy your resource group as an ARM template.

## Stage 8 - Docker

### Learning

- Docker: https://docs.docker.com/docker-for-windows/
- Container: https://docs.microsoft.com/en-us/dotnet/core/docker/build-container
- Container: https://docs.docker.com/engine/examples/dotnetcore/
- Container: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-2.2
- .NET Core Docker Runtimes: https://hub.docker.com/_/microsoft-dotnet-core-runtime/

### Tasks

- Docker: Install Docker for Windows locally with Kubernetes
- Processor: Convert your consumer processor back to a .Net Core command line and create a Docker image from it.
- Container: Run your processor image as a Linux container on your local Docker/Kubernetes

## Stage 9 - Azure Kubernetes Service

### Learning

- Azure Kubernetes Service: https://azure.microsoft.com/en-gb/services/kubernetes-service/
- Azure Container Registry: https://azure.microsoft.com/en-gb/services/container-registry/
- DevOps: https://docs.microsoft.com/en-us/azure/devops/pipelines/languages/docker?view=azure-devops
- Kubernetes: https://kubernetes.io/docs/concepts/overview/what-is-kubernetes/
- Kubernetes: https://kubernetes.io/docs/concepts/workloads/controllers/deployment/
- Kubernetes: https://kubernetes.io/docs/reference/kubectl/overview/

### Tasks

- Create Access policies in order to setup AKS:
 - 1. Go to Resource groups in Azure portal and choose "dev-astra-global" => dev-astra-vault-pri => Access policies
 - 2. Add access policy as you see in the image below. make sure to select Get and List for Secret permissions
 - 3. Go to Secrets and get the values for ServicePrincipalApplicationId and ServicePrincipalSecret

- Azure Kubernetes Service:
 - 1. Use an existing Service Principal
    - Client Id of ServicePrincipalApplicationId
    - Client Secret of ServicePrincipalSecret

- Azure Container Registry: Build your Docker image with Azure DevOps and publish it to our existing Azure Container Registry

- Kubernetes Deployment: Deploy your image to AKS as an Deployment with one pod using Azure DevOps and kubectl.

==================================

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

Abstractions be Damned!
- MessageBroker is what sends and receives messages
- MessageConsumer is solely responsible for consuming the messages
- MessageService takes messages from the broker and feeds them to the consumer