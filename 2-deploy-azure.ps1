Write-Output "********************************************"
Write-Output "Starting"
Write-Output "********************************************"

#Get environment variables
$app = [Environment]::GetEnvironmentVariable("app", [System.EnvironmentVariableTarget]::User)
$region = [Environment]::GetEnvironmentVariable("region", [System.EnvironmentVariableTarget]::User)
$group = [Environment]::GetEnvironmentVariable("group", [System.EnvironmentVariableTarget]::User)
$environment = [Environment]::GetEnvironmentVariable("environment", [System.EnvironmentVariableTarget]::User)
$logs = [Environment]::GetEnvironmentVariable("logs", [System.EnvironmentVariableTarget]::User)
$registry = [Environment]::GetEnvironmentVariable("registry", [System.EnvironmentVariableTarget]::User)

# Print all variables
Write-Host "Received Variables:"
Write-Host "app: $app"
Write-Host "region: $region"
Write-Host "group: $group"
Write-Host "environment: $environment"
Write-Host "logs: $logs"
Write-Host "registry: $registry"

# Check if any variables are empty and display them
$emptyVariables = @()
if ([string]::IsNullOrEmpty($app)) {
    $emptyVariables += "app"
}
if ([string]::IsNullOrEmpty($region)) {
    $emptyVariables += "region"
}
if ([string]::IsNullOrEmpty($group)) {
    $emptyVariables += "group"
}
if ([string]::IsNullOrEmpty($environment)) {
    $emptyVariables += "environment"
}
if ([string]::IsNullOrEmpty($logs)) {
    $emptyVariables += "logs"
}
if ([string]::IsNullOrEmpty($registry)) {
    $emptyVariables += "registry"
}

# Display empty variables
if ($emptyVariables.Count -gt 0) {
    $emptyVariablesMessage = "Empty Variables: $($emptyVariables -join ', ')"
    Write-Host $emptyVariablesMessage
    Write-Host "Script will exit due to empty variables."
    exit 1
}


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
az deployment group create -n $app -g $group --template-file .\deploy\main.bicep -p containerImageOrder=$registry.azurecr.io/order:latest containerImagePayment=$registry.azurecr.io/payment:latest containerPort=80 registry=$registry.azurecr.io registryUsername=$registryID registryPassword=$registryKey appName=$app

Write-Output "********************************************"
Write-Output  "                    Done                  *"
Write-Output "********************************************"