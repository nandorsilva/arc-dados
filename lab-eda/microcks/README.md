# Lab Eda

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

![Cluster Mongo db](../content/kafka-metrics.png)

Executando o docker compose

```
docker-compose up -d
```

Verificando se os containers foram criados com sucesso

```
 docker container ls
```
Verificando as imagens que foram feitas download do docker-hub
```
 docker image ls
```

# O que acontenceu ?


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
