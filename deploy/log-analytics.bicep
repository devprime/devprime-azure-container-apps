// *******************
// Log Analytics
// *******************
param location string
param name string

resource log 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: name
  location: location
  properties: any({
    retentionInDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
}


output loganalyticsUser string = log.properties.customerId
output loganalyticsPassword string = log.listKeys().primarySharedKey
