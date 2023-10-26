$dt=(Get-Date -UFormat %m)+(Get-Date -UFormat %d)+(Get-Date -UFormat %Y)
$app = "appdevprime"
$region = "eastus"
$group =  $app + "group"
$environment = $app + "environment"
$logs = $app + "logs"
$registry =  $app.ToLower() + "registry"

Write-Output "****************************"
echo "Try docker login"
docker login

echo "****************************"
echo "Try creating Resource Group "
echo "****************************"
az group create -n $group -l $region


echo "****************************"
echo "Try creating Container Registry"
echo "****************************"

az acr create -n $registry -g $group --sku Basic --admin-enabled true
# az acr list -g appdevprimegroup --output table
# az acr show -g appdevprimegroup -n $registry

echo "***********************************"
echo "Getting Azure ACR Credentials: $registry"
echo "***********************************"

$maxAttempts = 10
$retryInterval = 30
$acrpass = $null


for ($i = 1; $i -le $maxAttempts; $i++) {
    try {
        $acrpass = (az acr credential show -n $registry --query 'passwords[0].value' --output tsv)
        if ($acrpass) {
            Write-Host "ACR password obtained successfully: $acrpass"
            break
        } else {
            Write-Host "Attempt $i of $maxAttempts - Unable to obtain ACR password."
        }
    } catch {
        Write-Host "Error while running 'az acr credential' command. Attempt $i of $maxAttempts - The command failed."
    }

    if ($i -lt $maxAttempts) {
        Start-Sleep -Seconds $retryInterval
    }
}

if ($acrpass -eq $null) {
    Write-Host "Unable to obtain ACR password after $maxAttempts attempts."
}


echo "***********************************"
echo "Creating Docker Credentials: $registry"
echo "***********************************"
az acr login -n $registry -u $registry -p $acrpass


echo "***********************************"
echo "Buiding Order Microservices"
echo "***********************************"
docker build .\order\. -t order:latest
docker tag order:latest "$registry.azurecr.io/order:latest"


echo "***********************************"
echo "Push Order to Container Registry"
echo "***********************************"
docker push "$registry.azurecr.io/order:latest"

echo "***********************************"
echo "Buiding Payment Microservices"
echo "***********************************"
docker build .\payment\. -t payment:latest
docker tag payment:latest "$registry.azurecr.io/payment:latest"


echo "***********************************"
echo "Push Payment to Container Registry"
echo "***********************************"
docker push "$registry.azurecr.io/payment:latest"


echo "***********************************"
echo  "            DONE "
echo "***********************************"