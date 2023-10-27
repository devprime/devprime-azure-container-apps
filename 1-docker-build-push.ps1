$dt=(Get-Date -UFormat %m)+(Get-Date -UFormat %d)+(Get-Date -UFormat %Y)
$dtfull= Get-Date -Format "yyyyMMddHHmmss"

$app = "appdevprime"
$region = "eastus" #eastus #centralus #eastus2 #brazilsouth
$group =  $app + "group"
$environment = $app + "environment"
$logs = $app + "logs"
$registry =  $app.ToLower() + "registry"

Write-Output "****************************"
echo "Try docker login"
docker login

echo "****************************"
echo "Try creating Resource Group "
echo "Group: $group"
echo "Region: $region"
echo "****************************"
az group create -n $group -l $region


echo "****************************"
echo "Try creating Container Registry"
echo "Name:$registry"
echo "Group:$group"
echo "****************************"

az acr create -n $registry -g $group --sku Basic --admin-enabled true
# az acr list -g appdevprimegroup --output table
# az acr show -g appdevprimegroup -n $registry

echo "***********************************"
echo "Getting Azure ACR Credentials"
echo "ACR: $registry"
echo "***********************************"

$maxAttempts = 30
$retryInterval = 60
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
echo "Creating Docker Credentials:"
echo "Registry:$registry"
echo "Credential:$acrpass"
echo "***********************************"
#az acr login -n $registry -u $registry -p $acrpass


$maxAttempts = 20
$retryIntervalSeconds = 30

for ($i = 1; $i -le $maxAttempts; $i++) {
    $loginResult = az acr login -n $registry -u $registry -p $acrpass
    if ($loginResult -match "Error response from daemon: login attempt to .* failed with status: 404 Not Found") {
        Write-Host "Received a 404 error. Continuing to retry."
    } elseif ($loginResult -match "Login failed.") {
        Write-Host "Login failed."
        Write-Host "Connection attempt $i failed. Retrying in $retryIntervalSeconds seconds..."
        Start-Sleep -Seconds $retryIntervalSeconds
    } else {
        Write-Host "Connected to the registry successfully."
        break  # Exit the loop if the connection is successful
    }
}

if ($i -eq ($maxAttempts + 1)) {
    Write-Host "Failed to connect to the registry after $maxAttempts attempts."
    # Add additional code to handle the failure if necessary.
}





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