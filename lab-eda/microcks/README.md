# Lab Eda - Microcks


## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**


## Pré-requisitos?
* Docker
* Docker-Compose


# Instalação Kafka 

[LAB EDA](lab-eda//README.md)


## Instalando o Microcks

Entre na pasta microcks



*** Limpando os deploy anteriormente
```

//Entrar na pasta lab-eda
cd lab-eda 
```

Execute o docke-compose


```
docker-compose -f ambiente/docker-compose.yaml  up -d zookeeper kafka-broker mongo keycloak postman app async-minion 

```

O que aconteceu ??

Acessando http://localhost:8080/

* Username: admin
* Password: microcks123


## Vamos subir o Arquivo AsyncAPI ?

No menu `Importers >> Upload` realizar o upload do arquivo `asyncAPI/microcks.yml`

Vamos observar....

Listando os tópicos kafka do mock

```
docker exec -it kafka-broker /bin/bash
kafka-topics --bootstrap-server localhost:9092 --list 
```

Consumindo as mensagens do tópico mock

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic <<nome do tópico>>
```

> https://microcks.io/documentation/using/advanced/templates/