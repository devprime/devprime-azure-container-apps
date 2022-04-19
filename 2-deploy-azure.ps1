$app = "appdevprime"
$region = "eastus"
$group =  $app + "group"
$environment = $app + "environment"
$logs = $app + "logs"
$registry =  $app.ToLower() + "registry"




Write-Output "********************************************"
Write-Output "Getting Credentials from Container Registry"
$registryID=$(az acr credential show -n $registry --query username -o tsv)
$registryKey=$(az acr credential show -n $registry --query "passwords[0].value" -o tsv)

Write-Output "********************************************"
Write-Output  "Registry:" $registry
Write-Output  "User:" $registryID
Write-Output  "Password:" $registryKey


Write-Output "********************************************"
Write-Output  "Starting deploy..."
az deployment group create -n $app -g $group --template-file .\deploy\main.bicep -p containerImageOrder=$registry.azurecr.io/order:latest containerImagePayment=$registry.azurecr.io/payment:latest containerPort=80 registry=$registry.azurecr.io registryUsername=$registryID registryPassword=$registryKey

Write-Output "********************************************"
Write-Output  "                    Done                  *"
Write-Output "********************************************"