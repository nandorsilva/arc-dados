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
