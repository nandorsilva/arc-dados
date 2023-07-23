# Lab Eda - AsyncAPI


## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**
> 


## Pré-requisitos?
* Docker
* Docker-Compose
* Node.js v12.16 and higher
* Npm v6.13.7 and higher

# Instalação Kafka 

[LAB EDA](lab-eda//README.md)


# Criando um arquiro AsynAPI

https://studio.asyncapi.com/


# Criando o template html pelo AsyncAPI


> https://nodejs.org/en/download

**Instalando o gerador de tamplate html**

```
npm install -g @asyncapi/generator
```


Gerando o template html baseado no asyncAPI

```
 ag lab-eda/asyncAPI/estoque.yaml @asyncapi/html-template -o lab-eda/asyncAPI/templatehtml 
```

ag lab-eda/asyncAPI/estoque.yaml @asyncapi/java-spring-template


>https://www.asyncapi.com/