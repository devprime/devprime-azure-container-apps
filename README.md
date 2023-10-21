# Speeding Up Microservices with Azure Container Apps and DevPrime

***Introduction***
[Devprime](https://devprime.io) is a platform that accelerates software developer productivity and saves around 70% of the cost in backend software development by offering a modern software architecture design, components with intelligent behaviors, accelerators, and continuous updates.

Use the Devprime platform to accelerate software modernization and the development of cloud-native Microservices and event-driven APIs using a set of ready-to-use resources. Develop your first microservice in 30 minutes and simplify modernization and digital transformation.

Azure Container Apps offers a serverless container approach, allowing you to deploy microservices without the need for Kubernetes.

***Objective***
The objective of this tutorial is to demonstrate the development and deployment of two microservices using the [DevPrime platform](https://devprime.io) and utilizing the Azure Container Apps environment and related services such as Azure Container Registry (ACR), Azure Log Analytics, Azure Container Apps Environment, Azure CosmosDB with MongoDB, and Azure EventHub with Kafka.

The deployment scripts are structured in PowerShell and Bicep and aim to enable the demonstration of this scenario for software developers. In a production environment, it is necessary to implement a DevOps process and use other resources such as Azure Key Vault, API Management, Web Application Firewall, which are not part of this initial demonstration.

The image below demonstrates how the final environment will look after following all the procedures to create the environment and deploy the microservices.

![Azure Services](/public-images/azure-aca-01.png)

***Checklist and Initial Environment Setup***

- Install [.NET SDK 7 (Linux, macOS, and Windows)](https://dotnet.microsoft.com/en-us/download) or higher.
- Visual Studio Code / Visual Studio 2023.
- GIT installed.
- An active account on [Microsoft Azure](https://azure.com).
- An active account on the [DevPrime platform](https://devprime.io).
- Access Devprime's documentation to understand how to create [the first microservice](https://docs.devprime.io/quick-start/creating-the-first-microservice/).
- Acquire a [Devprime usage license](https://devprime.io/pricing).
- Install and authenticate [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/) (`az login`).
- Interactive Azure CLI authentication (`az login --scope https://management.core.windows.net//.default`).
- Install and authenticate [Devprime CLI](https://docs.devprime.io/quick-start/install-devprime-cli/) (`dp auth`).
- Install and authenticate Docker (`docker login`).
- Install [PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.2) on Windows, Linux, and macOS.
- Install Microsoft Bicep (`az bicep install`).

In this article, we will use two microservices built using the DevPrime platform and implemented as described in the article [Asynchronous Microservices Communication](https://docs.devprime.io/examples/stream/rabbitmq/asynchronous-microservices-communication/). You can manually implement the example or directly follow the provided code on GitHub.

***Getting Started***

a) Clone the project from GitHub:
   `git clone https://github.com/devprime/azure-container-apps-bicep`

b) Check the main folder with the Order and Payment items. Each folder contains a development microservice using the DevPrime platform.

![Locally Cloned Folder](/public-images/azure-aca-02.png)

c) Access the 'main' folder and add your license using the Devprime CLI.
   `dp stack`

After running the command, the following files will be modified:
- 'order\src\App\appsettings.json'
- 'payment\src\App\appsettings.json'

**Local Database and Stream Credentials**

To run the project locally, you need to [set up a local environment with Docker](https://docs.devprime.io/quick-start/docker/) and activate the MongoDB and Kafka containers, along with the "orderevents" and "paymentevents" topics in Kafka. The next step is to edit the configuration files of each microservice with the local MongoDB and Kafka credentials by locating the 'State' and 'Stream' keys.

Open the configuration file in Visual Studio Code:
`code order\src\App\appsettings.json`
`code payment\src\App\appsettings.json`

**Running the Microservice Locally**

Open two terminal tabs and run each of the microservices by navigating to the Order or Payment folder with the following command:
`.\run.ps1` or `./run.sh` (Linux, macOS)

***Exporting Microservices Configuration***

Our deployment script requires some parameters. To do this, go to the 'order' folder and execute the Devprime CLI export command to create a deployment file. Repeat the same process in the 'payment' folder. We will copy some parameters.
`dp export kubernetes`

Now return to the root folder and open the files to observe the parameters that will be sent during the deployment of Azure Container Apps. Look for the 'env:' key in the following files:
`code order\.devprime\kubernetes\deployment.yml`
`code payment\.devprime\kubernetes\deployment.yml`

***Introduction to Environment Variables***
Environment variables contain configuration parameters that will be passed to the Azure Container Apps instance.

![Environment Variables](/public-images/azure-aca-03.png)

**Configuring Environment Variables**

In conjunction with the example project, we are providing PowerShell and Bicep-based scripts to facilitate the deployment of the entire environment in Azure.

a) Edit the files 1-docker-build-push.ps1, 2-deploy-azure.ps1, and 3-cleanup.ps1 by defining a new value in the $app variable. Do not use special characters.
   `code .\1-docker-build-push.ps1`
   `code .\2-deploy-azure.ps1`
   `code .\3-cleanup.ps1`

b) Edit the `order/.devprime/kubernetes/deployment.yml` file, locate the "devprime_app" key, and then the "license" key, and copy the content to use in the next step.

c) Edit the `deploy\main.bicep` file and locate the line `param DevPrime_License string` and include the Devprime license code.
   `code deploy\main.bicep`
```

Agora, o texto deve ser exibido corretamente no GitHub com as quebras de linha apropriadas.

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
- Add Azure Key Vault

**Additional Research**

[Azure Container Apps documentation](https://docs.microsoft.com/en-us/azure/container-apps/)

[How to deploy Azure Container Apps with Bicep](https://www.thorsten-hans.com/how-to-deploy-azure-container-apps-with-bicep/)

[Deploy to Azure Container App from using a CI/CD Azure DevOps](https://thomasthornton.cloud/2022/02/11/deploy-to-azure-container-app-from-azure-container-registry-using-a-ci-cd-azure-devops-pipeline-and-azure-cli%EF%BF%BC/)

[How to Build and Deliver Apps Fast and Scalable with Azure Container Apps](https://www.youtube.com/watch?v=b3dopSTnSRg)

[Output connection strings and keys from Azure Bicep](https://blog.johnnyreilly.com/2021/07/07/output-connection-strings-and-keys-from-azure-bicep/)

[CosmosDB Bicep](https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb/manage-with-bicep)

**Trademarks**
- This project may contain trademarks or logos for projects, products, or services
- This sample code is proprietary to Devprime 
