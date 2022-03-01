$app = "appdevprime"
$region = "Canada Central"
$group =  $app + "group"
$environment = $app + "environment"
$logs = $app + "logs"
$registry =  $app.ToLower() + "registry"

echo "************************"
echo "Deleting Resource Group:$group" 
az group delete -n $group