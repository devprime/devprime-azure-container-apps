$app = [Environment]::GetEnvironmentVariable("app", [System.EnvironmentVariableTarget]::User)
$region = [Environment]::GetEnvironmentVariable("region", [System.EnvironmentVariableTarget]::User)
$group = [Environment]::GetEnvironmentVariable("group", [System.EnvironmentVariableTarget]::User)
$environment = [Environment]::GetEnvironmentVariable("environment", [System.EnvironmentVariableTarget]::User)
$logs = [Environment]::GetEnvironmentVariable("logs", [System.EnvironmentVariableTarget]::User)
$registry = [Environment]::GetEnvironmentVariable("registry", [System.EnvironmentVariableTarget]::User)

echo "***********************************************"
echo "Deleting Resource Group:$group" 
echo "***********************************************"
az group delete -n $group





