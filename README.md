# Microservices with Azure Container Apps and DevPrime 
[DevPrime](https://devprime.io) accelerates application delivery and development of Event-Driven, Cloud-Native Microservices and APIs using a Stack with accelerators, ready-made features. Develop your first microservice in 30 minutes and simplify modernization and digital transformation.

Azure Container Apps offers a serverless approach to publishing microservices without the need to use Kubernetes. In this tutorial, we demonstrate publishing two microservices developed using the [DevPrime platform](https://devprime.io).

The implementation of this environment involving two microservices [DevPrime](https://devprime.io) and Azure Container Apps will additionally use Azure Container Registry (ACR), Azure Log Analytics, Azure Container Apps Environment, Azure CosmosDB, Azure EventHub services.

The image below demonstrates how the final environment will look after we start all the procedures for creating the environment and publishing the microservices. 

![Azure Services](/public-images/azure-aca-01.png)

**Items needed in your environment**
- Install .NET SDK 6 or higher
- Visual Studio Code
- An active account on [Microsoft Azure](https://azure.com)
- An active account on the platform [DevPrime](https:/devprime.io) and license to use Devloper or Enterprise.
- [DevPrime CLI](../../../getting-started/) installed and active (`dp auth`)
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

c) Enter the 'order' folder and add your Devprime license. After executing the command it will change
the file 'order\src\App\appsettings.json'
`dp license`
d) Enter the 'payment' folder and add your Devprime license. After running it will change
the file 'order\src\App\appsettings.json'
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

Agora retorne a pasta raiz e abra os arquivos para observar os parâmetros que serão enviados
durante do deployment do Azure Container Apps. Visualize a chave 'env:' nos arquivos abaixo.
`code order\.devprime\kubernetes\deployment.yml`
`code payment\.devprime\kubernetes\deployment.yml`

**[Environment variables**
Ao executar os microsserviços Order e Payment na instância do Azure Container Apps é necessário configurar as variáveis de ambiente. Esse procedimento é muito parecido com o utilizado no Docker e Kubernetes e você poderá ter uma visualização na imagem abaixo.
![Environment variables](/public-images/azure-aca-03.png)

**Definindo as variáveis de ambiente**
a) Edite o arquivo 1-docker-build-push.ps1, 2-deploy-azure.ps1 e 3-cleanup.ps1 setando um novo valor na variável $app. Não utilize caracteres especiais.
b) Edite o arquivo deploy\main.bicep para alterar as configurações das variáveis de ambiente. 
`code deploy\main.bicep`
c) Copie o conteúdo da chave 'devprime_app' no arquivo 'order\.devprime\kubernetes\deployment.yml' em Order e altere no arquivo deploy\main.bicep na chave do microsserviço Order. Observe que no main.bicep nós criaremos duas instâncias do Azure Container Apps e você deve repetir os pasos no Payment.
```
// Container Apps: Order
// Container Apps: Payment
``` 
Nesse exemplo não alteraremos outras configurações. Caso necessite definir mais parâmetros para a sua aplicação repita o procedimento as outras chaves.

**Executando a criação do ambiente no Azure Container Apps**
Nós executaremos os scripts para que possa acompanhar passo a passo. Ao final se tudo correr bem você já terá nos logs a url do Azure Container Apps e consultará os serviços no portal do Azure.

a) Inicieremos com a criação dos serviços Azure Resource Group, Azure Container Registry (ACR), Docker Build e Push das imagens dos microsserviços para o repostório privado no ACR.
`.\1-docker-build-push.ps1`

b) Agora utillizaremos o bicep para criar Azure Container Apps, Azure Container App Environment, Azure CosmosDB, Log Analytics, Event Hubs.
`.\2-deploy-azure.ps1`

**Acessando os microsservices no Azure Container Apps**
Em nosso exemplo ao criar os serviços no Container Apps nós estamos utilizando a opção de receber requests (ingress) por meio de um endpoint público. 

As urls abaixo são exemplos dos acessos disponibilizados. Obtenha os seus.
- https://appdevprimeorder.calmbush-62be1470.canadacentral.azurecontainerapps.io
![Microservices Order](/public-images/azure-aca-04.png)

- https://appdevprimepayment.calmbush-62be1470.canadacentral.azurecontainerapps.io
![Microservices Payment](/public-images/azure-aca-05.png)

Ao fazer um post na API do Order ele vai processar a regra de negócio, persistência no mongodb (Azure CosmosDB) e depois emitirá um evento pelo Kafka (Azure EventHub).

O segundo microsserviços regiará ao evento e efetuará o seu ciclo natural de processamento conforme a regra de negócio implementada.

**Excluindo todo o ambiente criado**
Para excluir todos os serviços criados no Azure execute o script abaixo. Antes de confirmar certifique-se sobre o nome do Resource Group criado nessa demonstração
`.\3-cleanup.ps1`


**Sugestão para próximos passos**
- Automatize esse processo utilizando Azure DevOps, Github...
- Adicione uma configuração de segurança na exposição das API's
- Adicione um serviço do Azure API Management

**Para saber mais:**
[Azure Container Apps documentation](https://docs.microsoft.com/en-us/azure/container-apps/)
[How to deploy Azure Container Apps with Bicep](https://www.thorsten-hans.com/how-to-deploy-azure-container-apps-with-bicep/)
[Deploy to Azure Container App from using a CI/CD Azure DevOps](https://thomasthornton.cloud/2022/02/11/deploy-to-azure-container-app-from-azure-container-registry-using-a-ci-cd-azure-devops-pipeline-and-azure-cli%EF%BF%BC/)
[How to Build and Deliver Apps Fast and Scalable with Azure Container Apps](https://www.youtube.com/watch?v=b3dopSTnSRg)
[Output connection strings and keys from Azure Bicep](https://blog.johnnyreilly.com/2021/07/07/output-connection-strings-and-keys-from-azure-bicep/)
[CosmosDB Bicep](https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb/manage-with-bicep)
