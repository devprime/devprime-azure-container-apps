$app = "appdevprime"
$region = "eastus"
$group =  $app + "group"
$environment = $app + "environment"
$logs = $app + "logs"
$registry =  $app.ToLower() + "registry"

echo "****************************"
echo "Try docker login"
docker login

echo "****************************"
echo "Try creating Resource Group"
az group create -n $group -l $region


echo "****************************"
echo "Try creating Container Registry"
az acr create -n $registry -g $group --sku Basic --admin-enabled true
az acr show  -n $registry -g $group 


echo "***********************************"
echo "Creating Docker Credentials"
# az acr login -n $registry --expose-token
az acr login -n $registry


echo "***********************************"
echo "Buiding Order Microservices"
docker build .\order\. -t order:latest
docker tag order:latest "$registry.azurecr.io/order:latest"


echo "***********************************"
echo "Push Order to Container Registry"
docker push "$registry.azurecr.io/order:latest"

echo "***********************************"
echo "Buiding Payment Microservices"
docker build .\payment\. -t payment:latest
docker tag payment:latest "$registry.azurecr.io/payment:latest"


echo "***********************************"
echo "Push Payment to Container Registry"
docker push "$registry.azurecr.io/payment:latest"


echo "***********************************"
echo  "            DONE "
echo "***********************************"