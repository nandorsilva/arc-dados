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
cd ambiente
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


---

* 3.1. [LAB EDA - API](kafka-net/README.md)
* 3.2. [LAB EDA - AsyncAPI](asyncAPI/README.md)
* 3.3. [LAB EDA - Event-Catalog](event-catalog//README.md)
* 3.4. [LAB EDA - Microcks](microcks/README.md)
* 3.5. [LAB EDA - Kafka Conect](kafka-conect/README.md)
* 3.6. [LAB EDA - Jaeger](jaeger/README.md)
* 3.7. [LAB EDA - Ksqldb](ksql/README.md)