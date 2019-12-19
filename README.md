## Project Structure and Rational

### A Little About Cats
This project is a simple manager for all the cats in your life. It will let you create and edit a cat's name and it's fictional (or sometimes real) ability. Because...why not? And who doesn't love cats?!

### Manifests
Contains the container manifest and environment settings for Docker Compose, and Kubernetes service and deployment definitions.

### Starter.API
The Web API that exposes the CRUD operations for managing cats. Only single controller is implemented, creatively called ```CatController```.

### Starter.API.Tests
The tests for the ```Starter.API``` project. Contains the tests for the ```CatController``` class.

### Starter.Azure.Populate
Simple console application to create some sample data in Azure table storage.

### Starter.Bootstrapper
Encapulates the dependency resolver used for dependeny injection in all the projects. This is a separate project so the rest of the projects are not tied to a specific dependency injection framework. The current implementation is based on the built in .NET Core dependency injection.

### Starter.Broker.Azure
The implementation of the ```IMessageBroker``` from ```Starter.Data``` using Azure service bus.

### Starter.Broker.Azure.Tests
The tests for the ```Starter.Broker.Azure``` project. Contains tests for the ```AzureMessageBroker``` class.

### Starter.Build
PowerShell scripts for compiling, and running the project. As well as links to the container manifests, and the Azure pipeline.

### Starter.Configuration
Contains the configuration models, the application settings, and the service to read the configuration.

### Starter.Consumer
Exposes the ```MessageService``` to run as a console project through ```IHostedService```.

### Starter.Consumer.Azure
The implementation of the ```IMessageConsumer``` from ```Starter.Data``` as an Azure function.

### Starter.Data
Encapsulates the business logic, data models and interfaces used within the whole project.

### Starter.Data.Tests
The tests for the ```Starter.Data``` project. Contains tests for the ```MessageConsumer```, ```MessageService```, ```CatService``` and the WPF view model.

### Starter.Framework
Encapuslates the API client, logging, and useful extension methods.

### Starter.Framework.Tests
The tests for the ```Starter.Framework``` project. Contains tests for the API client, and all the extension methods.

### Starter.Mocks
Contains the ```Moq``` based mock objects used in the other test projects in the solution.

### Starter.Repository
Encapsulates the data access for the application. Implements the CRUD operations for the cat data using Azure table storage.

### Starter.Repository.Tests
The tests for the ```Starter.Repository``` project. Contains tests for the ```CatRepository``` class.

### Starter.Resources.Azure
Contains all the ARM templates and deploy script for creating the application resources in Azure.

### A Little Extra on the Messaging Setup
- The ```MessageBroker``` is responsible for sending and receiving the messages
- The ```MessageConsumer``` is solely responsible for consuming each message
- The ```MessageService``` takes messages from the broker and feeds them to the consumer

### Deployment
The application is setup to deploy through an ```Azure Pipeline``` every time there is code committed to the repository. The pipeline uses  multi-stages to build, and then deploy the applications to Kubernetes. The pipeline is defined in YAML and can be found in ```azure-pipelines.yml```

## Running the Project

### Requirements
- .NET Core 2.2
- PowerShell
- Azure CLI
- Azure PowerShell Module
- Azure Storage Emulator
- Docker Desktop

### Build and Run
- Start Docker Desktop
- Build in PowerShell
```
.\Starter.Build\build.ps1
```
- Run the API and the Consumer in Docker
```
.\Starter.Build\run.ps1
```
- Browse the API ```http://localhost:8080```

## Tests

### Running All Tests
- Start Microsoft Azure Storage Emulator
```
dotnet test
```

### Running Only Unit Test
```
dotnet test --filter TestCategory!="Integration"
```

## Deployment to Kubernetes

- Login to Azure
```
az login
```

- Get the Kubernetes Credentials
```
az aks get-credentials --resource-group boyankostadinov-newstartertask --name dev-astra-boyankostadinov-starter
```

- Deploy the Applications
```
kubectl apply -f Manifests\kubernetes-cluster.yml && kubectl get service starter-api --watch
```
- Stop the watch with Ctrl-C, copy the EXTERNAL-IP, and use the browser to access the API

- Open the Kubernetes Dashboard
```
az aks browse `
	--resource-group boyankostadinov-newstartertask `
	--name dev-astra-boyankostadinov-starter
```

- Delete the Kubernetes Services and Deployments
```
kubectl delete service --all && kubectl delete deployment --all
```

## Deployment of Azure Resources
- In PowerShell, connect to Azure
```
Connect-AzAccount
```

- In PowerShell, deploy the Azure ARM templates
```
.\Starter.Resources.Azure\deploy.ps1 -Location westeurope -ResourceGroup boyankostadinov-newstartertask
```

### Helpful Commands

- Login to Azure Container Registry
```
az acr login --name astracontainerregistry
```

- Tag Docker Images for Azure Container Registry
```
docker tag starter.api astracontainerregistry.azurecr.io/boyankostadinov-starter.api:latest
docker tag starter.messageconsumer.console astracontainerregistry.azurecr.io/boyankostadinov-starter.consumer:latest
```

- Push Docker Images for Azure Container Registry
```
docker push astracontainerregistry.azurecr.io/boyankostadinov-starter.api:latest
docker push astracontainerregistry.azurecr.io/boyankostadinov-starter.consumer:latest
```

- Run Local Docker Images Interactively
```
docker run -it --entrypoint sh starter.api
docker run -it --entrypoint sh starter.consumer
```

- Fix the Kubernetes Dashboard Permissions
```
kubectl create clusterrolebinding kubernetes-dashboard `
	-n kube-system `
	--clusterrole=cluster-admin `
	--serviceaccount=kube-system:kubernetes-dashboard
```

====================
## Requirements
====================
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