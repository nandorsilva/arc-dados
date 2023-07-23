# Lab Eda - kafka Net

## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**


### Pré-requisitos?
* Docker
* Docker-Compose


## Instalação Kafka 

[LAB EDA](lab-eda//README.md)


## Deploy Aplicação Net Producer


Criar uma imagem conforme o arquivo `dockerfile`, mas antes

**Crie uma conta no docker-hub**

https://hub.docker.com/


```

cd kafka-net

docker image build -f "kafka-net/Dockerfile" -t <<usuario>>/kafka-net:v1 .

```

**Depois da conta criada**


```
docker login --username=<<usuário>>
```

***Enviar para o repositorio***

```
docker push <<usuario>>/kafka-net:v1
```

>Apostila curso docker https://bit.ly/46Zcizg


Provisionando o container da imagem criada anteriormente.

> No arquivo docker-compose alterar a image que foi criada por vocês.

```

cd ambiente

docker-compose up -d kafka-net

docker container ls

```

http://localhost:5000/swagger/index.html


Verificando se as mensagens foram produzidas corretamente

```
docker exec -it  kafka-broker /bin/bash

kafka-console-consumer --bootstrap-server localhost:9092 --topic aluno --from-beginning

```
