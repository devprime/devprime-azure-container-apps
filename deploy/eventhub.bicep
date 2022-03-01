// *******************
// Event Hub
// *******************

@description('Eventhub account name')
param appEventHub string = 'appdevprimeeventhub'

@description('Location for the Eventhub account.')
param location string = resourceGroup().location


// Create a kafka topic
param eventHubOrder string = 'orderevents'
param eventHubPayment string = 'paymentevents'


var endpoint = '${eventHubNamespace.id}/AuthorizationRules/RootManageSharedAccessKey'

resource eventHubNamespace 'Microsoft.EventHub/namespaces@2021-06-01-preview' = {
  name: appEventHub
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
    capacity: 1
  }
  properties: {
    zoneRedundant: true
  }
}

//KafkaTopicName: Order
resource eventHubNamespaceName_eventHubName 'Microsoft.EventHub/namespaces/eventhubs@2021-06-01-preview' = {
  parent: eventHubNamespace
  name: eventHubOrder
  properties: {
    messageRetentionInDays: 7
    partitionCount: 1
    
  }
}

//KafkaTopicName: Payment
resource eventHubNamespaceName_eventHubNamePayment 'Microsoft.EventHub/namespaces/eventhubs@2021-06-01-preview' = {
  parent: eventHubNamespace
  name: eventHubPayment
  properties: {
    messageRetentionInDays: 7
    partitionCount: 1
  }
}


// Grant Listen and Send on our event hub
resource eventHubNamespaceName_eventHubName_ListenSend 'Microsoft.EventHub/namespaces/eventhubs/authorizationRules@2021-06-01-preview' = {
  parent: eventHubNamespaceName_eventHubName
  name: 'ListenSend'
  properties: {
    rights: [
      'Listen'
      'Send'
      'Manage'
    ]
  }
  dependsOn: [
    eventHubNamespace
  ]
}

resource eventHubNamespaceName_eventHubNamePayment_ListenSend 'Microsoft.EventHub/namespaces/eventhubs/authorizationRules@2021-06-01-preview' = {
  parent: eventHubNamespaceName_eventHubNamePayment
  name: 'ListenSendPayment'
  properties: {
    rights: [
      'Listen'
      'Send'
      'Manage'
    ]
  }
  dependsOn: [
    eventHubNamespace
  ]
}


//output
output eventHubOrderConnectionString string = listKeys(eventHubNamespaceName_eventHubName_ListenSend.id, eventHubNamespaceName_eventHubName_ListenSend.apiVersion).primaryConnectionString
output eventHubPaymentConnectionString string = listKeys(eventHubNamespaceName_eventHubNamePayment_ListenSend.id, eventHubNamespaceName_eventHubNamePayment_ListenSend.apiVersion).primaryConnectionString
output eventHubConnectionString string = '${listKeys(endpoint, eventHubNamespaceName_eventHubName.apiVersion).primaryConnectionString}'


