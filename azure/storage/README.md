
# Laboratórios do curso Arquitetura de DADOS

---
## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**
> 

---


## Pré-requisitos?
* Conta Azure

## Criando Storage Account


### Provisionando storage - Passo 1

![Criando storage account](/content/storage-01.png/)


### Configurando access tier

![Criando storage account](/content/storage-02.png/)

![Criando storage account](/content/storage-03.png/)


### Configurando Versionamento
![Criando storage account](/content/storage-04.png/)

### Configurando tags e concluindo
![Criando storage account](/content/storage-05.png/)


### Criando um container
![Criando storage account](/content/storage-06.png/)


### Sunbindo imagem
![Criando storage account](/content/storage-07.png/)

### Visualizando as versões das empresas
![Criando storage account](/content/storage-08.png/)

### Visualizando as mensagens do Event-Grid

https://github.com/Azure-Samples/azure-event-grid-viewer



> Uma pausa, para a criação do App registrations

### Criando App registrations

![Criando storage account](/content/storage-10.png/)

### Copie as informações do app registrations, Directory (tenant) ID e Application (client) ID

![Criando storage account](/content/storage-11.png/)


### Gerando evento do storage account
![Criando storage account](/content/storage-09.png/)


![Criando storage account](/content/storage-12.png/)

> https://viewerimage.azurewebsites.net/api/updates

![Criando storage account](/content/storage-13.png/)


![Criando storage account](/content/storage-14.png/)



### Testando o evento subindo uma imagem
![Criando storage account](/content/storage-15.png/)

### Testando o evento subindo uma imagem
![Criando storage account](/content/storage-16.png/)

### Filtrando o evento pelo conteiner imagem

> 
![Criando storage account](/content/storage-17.png/)


## Criando um azure function para resize imagem
![Criando storage account](/content/storage-18.png/)


![Criando storage account](/content/storage-19.png/)

![Criando storage account](/content/storage-20.png/)

![Criando storage account](/content/storage-21.png/)


## Criando conteiner no storage account para a imagem alterada
![Criando storage account](/content/storage-22.png/)

## Configurando as configurações para a aplicação

```
storageConnectionString=$(az storage account show-connection-string --resource-group aula-01-fia --name imagensfia --query connectionString --output tsv)


az functionapp config appsettings set --name functionimagemaula --resource-group aula-01-fia --settings AzureWebJobsStorage=$storageConnectionString THUMBNAIL_CONTAINER_NAME=thumbnails THUMBNAIL_WIDTH=100 FUNCTIONS_EXTENSION_VERSION=~2 FUNCTIONS_WORKER_RUNTIME=dotnet
```

## Restart do Function App após configuração dos configs
![Criando storage account](/content/storage-23.png/)


## Deploy aplicação Azure function

```
az functionapp deployment source config --name functionimagemaula --resource-group aula-01-fia --branch master --manual-integration --repo-url https://github.com/Azure-Samples/function-image-upload-resize
```

## Criando mais uma subscription para event-grid
![Criando storage account](/content/storage-24.png/)

![Criando storage account](/content/storage-25.png/)

![Criando storage account](/content/storage-26.png/)
