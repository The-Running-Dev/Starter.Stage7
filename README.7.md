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