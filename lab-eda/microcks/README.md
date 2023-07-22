# Lab Eda - Microcks

---
## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**
> 

---

---


## Pré-requisitos?
* Docker
* Docker-Compose


# Instalação Kafka 

[LAB EDA](lab-eda//README.md)


## Instalando o Microcks

Entre na pasta microcks


```
cd microcks

```

Execute o docker-compose

```
docker-compose up -d
```

O que aconteceu ??

## Vamos subir o Arquivo AsyncAPI ?

Vamos observar....

Listando os tópicos kafka do mock

```
kafka-topics --bootstrap-server localhost:9092 --list 
```

Consumindo as mensagens do tópico mock

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic <<nome do tópico>>
```
