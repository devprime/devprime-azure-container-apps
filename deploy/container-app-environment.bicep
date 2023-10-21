param name string
param location string
param logId string
param logKey string


resource env 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: name
  location: location
  properties: {
      appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logId
        sharedKey: logKey
      }
    }
  }
}

output id string = env.id
