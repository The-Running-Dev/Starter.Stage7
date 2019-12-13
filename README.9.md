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