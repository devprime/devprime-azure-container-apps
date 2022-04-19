//DevPrime
param DevPrime_License string ='put your devprime license'

//Param
param location string = resourceGroup().location
param appName string = 'appdevprime'
param appNameDB string = '${appName}cosmosdb'
param appEventHub string = '${appName}eventhub'



param containerImageOrder string
param containerImagePayment string
param containerPort int
param registry string
param registryUsername string
param registryPassword string

//Log Analytics
@description('Criando o Log Analytics')
module log 'log-analytics.bicep' = {
	name: 'log-analytics'
	params: {
      location: location
      name: '${appName}loganalytics'
	}
}
//Container App Environment
@description('Creating Container App Environment')
module containerAppEnvironment 'container-app-environment.bicep' = {
  name: 'container-app-environment'
  params: {
    name: '${appName}environment'
    location: location
    logId:log.outputs.loganalyticsUser
    logKey:log.outputs.loganalyticsPassword
  }
}


// CosmosDB
module cosmosdb 'cosmosdb.bicep' = {
  name: 'cosmosdb'
  params: {
    location: location
    primaryRegion: location
    accountName: appNameDB
    databaseName: appName
  }
}



module eventhub 'eventhub.bicep' = {
	name: 'eventhub'
	params: {
      location: location
			appEventHub: appEventHub

	}
}

// EventHub: Kafka
var eventHubconnection = eventhub.outputs.eventHubConnectionString
// CosmosDB: MongoDB
var dbconnection ='mongodb://${appNameDB}:${cosmosdb.outputs.primaryMasterKey}@${appNameDB}.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@${appNameDB}@'

// Container Apps: Order
module containerAppOder 'container-apps.bicep' = {
  name: 'order'
  params: {
    name: '${appName}order'
    location: location
    containerAppEnvironmentId: containerAppEnvironment.outputs.id
    containerImage: containerImageOrder
    containerPort: containerPort
    envVars: [
        {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Production'
        }

        {
          name: 'devprime_app'
          value: 'license=${DevPrime_License}|||debug=false|||debugstate=false|||debugstream=false|||debugweb=false|||showenviromentvariables=false|||tenancy=[enable=false,type=Shared,cache=State2,gateway=https://localhost:5003]|||idempotency=[Enable=false,Alias=State2,Duration=86400,Flow=backend,key=idempotency-key,Scope=all,Action=auto]'
        }

        {
          name: 'devprime_observability'
          value: 'enable=true|||trace=[enable=false,endpoint=http://localhost:9411/api/v2/spans]|||log=[enable=true,save=false,type=text,hidedetails=false,hidedatetime=false,showhttperrors=400,showappname=false,filesize=5242880,export=[enable=false,host=http://localhost:5341,type=seq,apikey=your api key,controllevelswitch=Information]]'
        }

        {
          name: 'devprime_web'
          value: 'url=http://*:80|||enable=true|||enableswagger=true|||postsuccess=201|||postfailure=500|||getsuccess=200|||getfailure=500|||patchsuccess=200|||patchfailure=500|||putsuccess=200|||putfailure=500|||deletesuccess=200|||deletefailure=500'
        }

        {
          name: 'devprime_stream1'
          value: 'alias=Stream1|||enable=true|||default=true|||streamtype=Kafka|||hostname=${appEventHub}.servicebus.windows.net|||user=$ConnectionString|||password=${eventHubconnection}|||port=9093|||retry=3|||fallback=State1'
        }

        {
          name: 'devprime_state1'
          value: 'alias=State1|||dbtype=mongodb|||connection=${dbconnection}|||timeout=5|||retry=2|||dbname=order|||isssl=true|||numberofattempts=4|||durationofbreak=45'
        }

        {
          name: 'Devprime_Custom'
          value: 'stream.orderevents=orderevents'
        }

        {
          name: 'DevPrime_Security'
          value: 'Production'
        }

        {
          name: 'DevPrime_Services'
          value: 'retry=3|||circuitbreak=45|||timeout=10|||connections=[granttype=client_credentials,name=Services1,]'
        }
    ]
    useExternalIngress: true
    registry: registry
    registryUsername: registryUsername
    registryPassword: registryPassword
  }
}

// Container Apps: Payment
module containerAppPayment 'container-apps.bicep' = {
  name: 'payment'
  params: {
    name: '${appName}payment'
    location: location
    containerAppEnvironmentId: containerAppEnvironment.outputs.id
    containerImage: containerImagePayment
    containerPort: containerPort
    envVars: [
        {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Production'
        }

        {
          name: 'devprime_app'
          value: 'license=${DevPrime_License}|||debug=false|||debugstate=false|||debugstream=false|||debugweb=false|||showenviromentvariables=false|||tenancy=[enable=false,type=Shared,cache=State2,gateway=https://localhost:5003]|||idempotency=[Enable=false,Alias=State2,Duration=86400,Flow=backend,key=idempotency-key,Scope=all,Action=auto]'
        }

        {
          name: 'devprime_observability'
          value: 'enable=true|||trace=[enable=false,endpoint=http://localhost:9411/api/v2/spans]|||log=[enable=true,save=false,type=text,hidedetails=false,hidedatetime=false,showhttperrors=400,showappname=false,filesize=5242880,export=[enable=false,host=http://localhost:5341,type=seq,apikey=your api key,controllevelswitch=Information]]'
        }

        {
          name: 'devprime_web'
          value: 'url=http://*:80|||enable=true|||enableswagger=true|||postsuccess=201|||postfailure=500|||getsuccess=200|||getfailure=500|||patchsuccess=200|||patchfailure=500|||putsuccess=200|||putfailure=500|||deletesuccess=200|||deletefailure=500'
        }

        {
          name: 'devprime_stream1'
          value: 'alias=Stream1|||enable=true|||default=true|||streamtype=Kafka|||hostname=${appEventHub}.servicebus.windows.net|||user=$ConnectionString|||password=${eventHubconnection}|||port=9093|||retry=3|||fallback=State1|||subscribe=[queues=orderevents]'
          
        }

        {
          name: 'devprime_state1'
          value: 'alias=State1|||dbtype=mongodb|||connection=${dbconnection}|||timeout=5|||retry=2|||dbname=payment|||isssl=true|||numberofattempts=4|||durationofbreak=45'
        }

        {
          name: 'Devprime_Custom'
          value: 'stream.paymentevents=paymentevents'
        }

        {
          name: 'DevPrime_Security'
          value: 'Production'
        }

        {
          name: 'DevPrime_Services'
          value: 'retry=3|||circuitbreak=45|||timeout=10|||connections=[granttype=client_credentials,name=Services1,]'
        }
    ]
    useExternalIngress: true
    registry: registry
    registryUsername: registryUsername
    registryPassword: registryPassword
  }
}

// Output
output containerAppOrder string = containerAppOder.outputs.fqdn
output containerAppPayment string = containerAppPayment.outputs.fqdn
output eventHubOrderConnectionString string = eventhub.outputs.eventHubOrderConnectionString
output eventHubPaymentConnectionString string = eventhub.outputs.eventHubPaymentConnectionString
output eventHubConnectionString string = eventhub.outputs.eventHubConnectionString
