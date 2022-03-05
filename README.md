# Microservices with Azure Container Apps and DevPrime 
[DevPrime](https://devprime.io) accelerates application delivery and development of Event-Driven, Cloud-Native Microservices and APIs using a Stack with accelerators, ready-made features. Develop your first microservice in 30 minutes and simplify modernization and digital transformation.

Azure Container Apps offers a serverless approach to publishing microservices without the need to use Kubernetes. In this tutorial, we demonstrate publishing two microservices developed using the [DevPrime](https://devprime.io) Platform.

The implementation of this environment involving two microservices [DevPrime](https://devprime.io) and Azure Container Apps will additionally use Azure Container Registry (ACR), Azure Log Analytics, Azure Container Apps Environment, Azure CosmosDB, Azure EventHub services.

The image below demonstrates how the final environment will look after we start all the procedures for creating the environment and publishing the microservices. 

![Azure Services](/public-images/azure-aca-01.png)

**Items needed in your environment**
- Install .NET SDK 6 or higher
- Visual Studio Code
- An active account on [Microsoft Azure](https://azure.com)
- An active account on the platform [DevPrime](https://devprime.io) and license to use Devloper or Enterprise.
- [DevPrime CLI](https://docs.devprime.tech/getting-started/) installed and active (`dp auth`)
- Azure CLI installed and active (`az login`)
- Active local docker
- Microsoft Powershell installed
- Microsoft Bicep installed ( `az bicep install`)
- GIT

In this article we will use two microservices built by DevPrime and implemented as presented in the article [Asynchronous Microservices Communication](https://docs.devprime.tech/how-to/asynchronous-microservices-communication/). You can run the above example in advance or go straight to the code provided by github.

This project uses powershell and bicep based scripts to create the Azure Container Apps environment in Azure. You can adapt the scripts as per your need.

**First steps**

a) Run a clone of the project on github

`git clone https://github.com/devprime-io/azure-container-apps-bicep`

b) Check the home folder with the Order and Payment items. Each such folder has a development microservice with the DevPrime platform.

![Folder cloned locally](/public-images/azure-aca-02.png)

c) Enter the 'order' folder and add your Devprime license. After executing the command it will change the file 'order\src\App\appsettings.json'

`dp license`

d) Enter the 'payment' folder and add your Devprime license. After running it will change the file 'payment\src\App\appsettings.json' 

`dp license`

**Local database and stream credentials**

To run the microservice locally, adding the credentials of a mongodb database and a kafka cluster in the order project and in the payment project, editing the 'appsettings.json' file as shown in the example below. At deployment time we will use the credentials of the Azure environment.

Optionally locate the 'State' and 'Stream' keys and change the values with mongodb and/or kafka service credentials in the 'order' and 'payment' folders

`code order\src\App\appsettings.json`

`code payment\src\App\appsettings.json`

**Running the microservice locally**

Enter the order or payment folder and run

`.\run.ps1 or ./run.sh (Linux, macOS)`

**Exporting microservices settings**
Enter the 'order' folder and run the DevPrime CLI export command to create a deployment file. Repeat the same procedure on the 'payment' folder. We will copy some parameters.

`dp export kubernetes`

Now return to the root folder and open the files to observe the parameters that will be sent
during deployment of Azure Container Apps. View the 'env:' key in the files below.

`code order\.devprime\kubernetes\deployment.yml`

`code payment\.devprime\kubernetes\deployment.yml`

**[Environment variables]**

When running the Order and Payment microservices on the Azure Container Apps instance, it is necessary to configure the environment variables. This procedure is very similar to the one used in Docker and Kubernetes and you can see a preview in the image below.

![Environment variables](/public-images/azure-aca-03.png)

**Setting the environment variables**

a) Edit the files 1-docker-build-push.ps1, 2-deploy-azure.ps1 and 3-cleanup.ps1 by setting a new value in the $app variable. Do not use special characters.

b) Edit the deploy\main.bicep file to change the environment variable settings.

`code deploy\main.bicep`

c) Copy the contents of the key 'devprime_app' in the file 'order\.devprime\kubernetes\deployment.yml' in Order and change in the file deploy\main.bicep in the microservice key Order. Note that in main.bicep we will create two instances of Azure Container Apps and you must repeat the steps in Payment.
```
// Container Apps: Order
// Container Apps: Payment
```
In this example we will not change other settings. If you need to define more parameters for your application, repeat the procedure for the other keys.

**Running environment creation in Azure Container Apps**
We'll run the scripts so you can follow along step by step. At the end, if everything goes well, you will already have the Azure Container Apps url in the logs and you will consult the services in the Azure portal.

a) We will start by creating the Azure Resource Group, Azure Container Registry (ACR), Docker Build and Push services from the microservices images to the private repository in ACR.

`.\1-docker-build-push.ps1`

b) Now we will use the bicep to create Azure Container Apps, Azure Container App Environment, Azure CosmosDB, Log Analytics, Event Hubs.

`.\2-deploy-azure.ps1`

**Accessing microservices in Azure Container Apps**

In our example when creating the services in Container Apps we are using the option to receive requests (ingress) through a public endpoint.

The urls below are examples of the accesses available. Get yours.

- https://appdevprimeorder.calmbush-62be1470.canadacentral.azurecontainerapps.io

![Microservices Order](/public-images/azure-aca-04.png)

- https://appdevprimepayment.calmbush-62be1470.canadacentral.azurecontainerapps.io

![Microservices Payment](/public-images/azure-aca-05.png)

When making a post in the Order API, it will process the business rule, persistence in mongodb (Azure CosmosDB) and then it will emit an event through Kafka (Azure EventHub).

The second microservices will respond to the event and perform its natural processing cycle according to the implemented business rule.

**Excluding all created environment**
To delete all services created in Azure run the script below. Before confirming make sure about the name of the Resource Group created in this demo

`.\3-cleanup.ps1`


**Suggestion for next steps**
- Automate this process using Azure DevOps, Github...
- Add a security setting in the API's exposure
- Add an Azure API Management service

**Additional Research**

[Azure Container Apps documentation](https://docs.microsoft.com/en-us/azure/container-apps/)

[How to deploy Azure Container Apps with Bicep](https://www.thorsten-hans.com/how-to-deploy-azure-container-apps-with-bicep/)

[Deploy to Azure Container App from using a CI/CD Azure DevOps](https://thomasthornton.cloud/2022/02/11/deploy-to-azure-container-app-from-azure-container-registry-using-a-ci-cd-azure-devops-pipeline-and-azure-cli%EF%BF%BC/)

[How to Build and Deliver Apps Fast and Scalable with Azure Container Apps](https://www.youtube.com/watch?v=b3dopSTnSRg)

[Output connection strings and keys from Azure Bicep](https://blog.johnnyreilly.com/2021/07/07/output-connection-strings-and-keys-from-azure-bicep/)

[CosmosDB Bicep](https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb/manage-with-bicep)
