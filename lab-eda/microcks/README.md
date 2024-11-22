# Lab Eda - Microcks


## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**


## Pré-requisitos?
* Docker
* Docker-Compose


# Instalação Kafka 


# [Para configurar o ambiente do Kafka, consulte o laboratório dedicado ao Kafka](../../lab-kafka/README.md)

## Instalando o Microcks


```bash
//Entrar na pasta lab-eda
cd lab-eda 
```

Execute o docke-compose no Terminal

```bash
docker compose -f ambiente/docker-compose.yaml  up -d zookeeper kafka-broker mongo keycloak postman app async-minion 

```

O que aconteceu ??

Acessando o endereço http://localhost:8080/

* Username: admin
* Password: microcks123


## Vamos subir o Arquivo AsyncAPI ?

No menu `Importers >> Upload` realizar o upload do arquivo que está dentro da pasta `asyncAPI/microcks.yml`

Vamos observar....

Listando os tópicos kafka gerados pelo Microcks

```bash
docker exec -it kafka-broker /bin/bash
kafka-topics --bootstrap-server localhost:9092 --list 
```

Consumindo as mensagens do tópico mock

```bash
kafka-console-consumer --bootstrap-server localhost:9092 --topic UsersignedupAPI2-0.1.1-sku
```

> https://microcks.io/documentation/references/templates/