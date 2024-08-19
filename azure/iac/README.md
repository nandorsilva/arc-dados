
# Laboratórios do curso Arquitetura de DADOS

---
## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**
> 

---


## Pré-requisitos?
* Conta Azure
* Instalação - https://learn.microsoft.com/en-us/powershell/azure/install-azps-windows?view=azps-12.2.0&tabs=powershell&pivots=windows-psgallery

## Deployando Arm Template 


Conectando na azure via power shell


```
Connect-AzAccount

```

Executando o arquivo power shell

```
.\storage.ps1
```

## Deployando Arm Template no portal


```
az group deployment create -g "aula-01-fia" --template-file storage.json 

```


> https://learn.microsoft.com/en-us/azure/azure-resource-manager/templates/syntax
> https://learn.microsoft.com/en-us/samples/browse/?expanded=azure&products=azure-resource-manager