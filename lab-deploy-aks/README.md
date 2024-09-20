# Lab Microservices Kafka-Net AKS

## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**


### Pré-requisitos?
* Docker
* Docker-Compose
* Kubernetes - AKS
* Kubectl


# Deploy Aplicação Net Producer Localmente



Provisionando o container da imagem fernandos/kafka-net.

> No arquivo docker-compose no caminho /lab-eda/ambiente alterar a image que foi criada por vocês.

```

cd lab-eda/ambiente/

docker compose up -d kafka-net kafka-broker zookeeper

docker container ls

```

http://localhost:5000/swagger/index.html


> Ambiente funcionando localmente

## Consumindo mensagens

```
docker exec -it kafka-broker /bin/bash

kafka-console-consumer --bootstrap-server localhost:9092 --topic aluno --from-beginning
```

---

# Deploy a aplicação em um Kubernetes aks.


### Alguns utilitários são necessários a instalação e configuração dos componentes, ou pode usar o shell da Azure
* Azure CLI
https://learn.microsoft.com/en-us/cli/azure/install-azure-cli

* Kubectl
https://kubernetes.io/docs/tasks/tools/install-kubectl-linux/


Entrando na conta da Azure

```
az login  --use-device-code 
```


Criando o resource group na Azure

```
az group create -n rg-fia -l EastUS2
```


# Criando o Azure Container Registry.

>Azure Container Registry (ACR) é um serviço do Microsoft Azure que permite armazenar e gerenciar imagens de contêiner de Docker de maneira privada e altamente segura.  https://azure.microsoft.com/en-us/products/container-registry


Verifica se o nome que vamos criar para o ACR está disponível

```
az acr check-name -n fiaacr
```

Criando o ACR

```
az acr create -g rg-fia -n fiaacr --sku Basic --admin-enabled false
```

Armazenando Id do ACR em uma variável local

```
ACR_ID=$(az acr show -n fiaacr -g rg-fia --query id -o tsv)
```


Executamos o aplicação localmente com a imagem chamada fernandos/kafka-net:v1, agora vamos taguiar com o nome
do nosso ACR

```
docker tag fernandos/kafka-net:v0 fiaacr.azurecr.io/kafka-net:v0
```

> Observer que o endereço do nosso ACR é fiaacr.azurecr.io, veja seu nome caso tenho alterado.


Logar no ACR

```
az acr login -n fiaacr
```

Enviando a imagem para o ACR


```
docker push fiaacr.azurecr.io/kafka-net:v0
az acr repository list --name fiaacr --output table

```


Criando o AKS (Kubernetes)

```
az aks create -n aksappkafka -g rg-fia --enable-managed-identity --attach-acr $ACR_ID --node-count 1 --generate-ssh-keys
```

> Se o SSH Keys já foi criado tire o atributo --generate-ssh-keys.

Conectando no AKS

```
az aks get-credentials --resource-group rg-fia --name aksappkafka --overwrite-existing
```

Criando o Deployment

```
cd ../../lab-deploy-aks/

kubectl apply -f deployment.yml

kubectl get namespace

kubectl get pods

 kubectl get pods -o jsonpath='{range .items[*]}{"\n"}{.metadata.name}{":\t"}{range .spec.containers[*]}{.image}{", "}{end}{end}'

```

Expondo o Deployment via serviço

```
 kubectl get services
 kubectl apply -f service.yml
```

Testando a API.Net

```
kubectl get services
```

http://<<ip do serviço>>/swagger/index.html
