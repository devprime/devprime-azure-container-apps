// params
// https://raw.githubusercontent.com/jamiemccrindle/bicep-app-service-container/main/infra/acr/main.bicep

@minLength(5)
@maxLength(50)
@description('Specifies the name of the azure container registry.')
param acrName string = 'acr001${uniqueString(resourceGroup().id)}' // must be globally unique

@description('Enable admin user that have push / pull permission to the registry.')
param acrAdminUserEnabled bool = true

@description('The owner of this ACR.')
param ownerPrincipalId string

@description('Specifies the Azure location where the acr should be created.')
param location string = resourceGroup().location

@allowed([
  'Basic'
  'Standard'
  'Premium'
])
@description('Tier of your Azure Container Registry.')
param acrSku string = 'Basic'

// azure container registry
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: acrName
  location: location
  sku: {
    name: acrSku
  }
  properties: {
    adminUserEnabled: acrAdminUserEnabled
  }
}

// assign an owner role to the ACR
resource ownerRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('${acr.id}/${ownerPrincipalId}/owner')
  scope: acr
  properties: {
    roleDefinitionId: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/8e3af657-a8ff-443c-a75c-2fe8c4bcb635'
    principalId: ownerPrincipalId
  }
}

// create a scope map for your repository
resource bicepAppServiceContainerScopeMap 'Microsoft.ContainerRegistry/registries/scopeMaps@2023-07-01' = {
  parent: acr
  name: 'bicepAppServiceContainer'
  properties: {
    actions: [
      'repositories/bicep-app-service-container/content/read'
      'repositories/bicep-app-service-container/metadata/read'
    ]
  }
}

resource bicepAppServiceContainerToken 'Microsoft.ContainerRegistry/registries/tokens@2023-07-01' = {
  parent: acr
  name: 'bicepAppServiceContainer'
  properties: {
    scopeMapId: bicepAppServiceContainerScopeMap.id
    status: 'enabled'
  }
}

output acrLoginServer string = acr.properties.loginServer
output acrName string = acrName
