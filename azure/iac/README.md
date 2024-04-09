
# Laboratórios do curso Arquitetura de DADOS

---
## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**
> 

---


## Pré-requisitos?
* Conta Azure

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