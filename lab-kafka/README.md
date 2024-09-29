
# LAB KAFKA

---
## Disclaimer
> **Esta configuração é puramente para fins de desenvolvimento local e estudos**
> 

---


## Pré-requisitos?
* Docker
* Docker-Compose

---

## Criando o ambiente Kafka com o docker compose


```
cd ..
cd lab-eda/ambiente
docker-compose up -d grafana prometheus jmx-kafka-broker zookeeper kafka-broker zoonavigator akhq

```

## O que acontenceu ?


## Acesso WebUI dos componentes


* AKHQ http://localhost:8080/ui
* ZooNavigator http://localhost:8000/
* Prometeus http://localhost:9090/
* jmx-prometheus-exporter http://localhost:5556/
* Grafana http://localhost:3000/login

## Acessos

ZooNavigator

```
zookeeper:2181
```

Grafana

* user : `admin`
* password : `kafka`

Verificando se os containers foram criados com sucesso

```
 docker container ls
```
Verificando as imagens que foram feitas download do docker-hub
```
 docker image ls
```

---

Vamos executar alguns comandos de dentro do container kafka-broker

Acessar o Shell do container kafka-broker

```
docker exec -it kafka-broker /bin/bash
```

# Criando nosso Primeiro tópico
```
kafka-topics --bootstrap-server localhost:9092 --topic alunos --create
```

Listando o tópico criado
```
kafka-topics --bootstrap-server localhost:9092 --list 
```

Alguém lembra das partições? Agora o tópico com mais de uma partição

```
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos --create --partitions 3
```
Esqueceu a configuração do tópico?

```
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos --describe
```

... e com fator de replicação

```
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos-factor --create --partitions 3 --replication-factor 2
```
...deu certo, porque ?

Agora vai dar certo...
```
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos-factor --create --partitions 3 --replication-factor 1
```


Criando tópicos com configurações

```
kafka-topics --bootstrap-server localhost:9092 --create --topic topico-config --partitions 3 --replication-factor 1

kafka-configs --bootstrap-server kafka:29092 --entity-type topics --entity-name topico-config --alter --add-config retention.ms=259200000

kafka-topics --bootstrap-server localhost:9092 --describe --topic topico-config
```

# Deletando um tópico

```
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos-factor --delete
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos-factor --describe
```

# Produzinho mensagens

```
kafka-console-producer --bootstrap-server localhost:9092 --topic alunos

>Minha primeira mensagem
>Melhor lab do brasil
>Eu sou o Fulano
>^C  (<- Ctrl + C is used to exit the producer)

```

Produzinho mensagens com acks

```
kafka-console-producer --bootstrap-server localhost:9092 --topic alunos --producer-property acks=all
```

Criando o tópico no momento de criar a mensagem

```
kafka-console-producer --bootstrap-server localhost:9092 --topic professor

kafka-topics --bootstrap-server localhost:9092 --topic professor --describe
```

> A Criação do tópico foi possivel pois a propriedade auto.create.topics.enable está com true.

O tópico foi criado com configurações default

Ver as configurações na pasta cat /etc/kafka/server.properties

Produzir mensagens habilitando a Key

```
kafka-console-producer --bootstrap-server localhost:9092 --topic alunos --property parse.key=true --property key.separator=:
>key:value
>aluno:fernando
```

# Consumindo mensagens

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic alunos
```

Abre outro terminal, entre no container e produza uma mensagem

```
//Entrando no containar em outro terminal

docker exec -it kafka1 /bin/bash

//Produzindo mensagens

kafka-console-producer --bootstrap-server localhost:9092 --topic alunos --property parse.key=true --property key.separator=:

>aluno:fernando
>aluno:felipe

```

Consumindo as mensagens desde o inicio

No primeiro terminal cancele o consumo das mensagens

```
>^C  (<- Ctrl + C is used to exit the producer)

kafka-console-consumer --bootstrap-server localhost:9092 --topic alunos --from-beginning

```

Consumindo mensagens mostrando algumas configurações tais como: `Key` e `Value`

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic alunos  --property print.timestamp=true --property print.key=true --property print.value=true --property print.partition=true --from-beginning

>^C  (<- Ctrl + C is used to exit the producer)

```

# Consumer group

Criando um consumer group

Consumindo as mensagens com um consumer group

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic alunos --group aplicacao-lab
```

Em um outro terminal....

Produzindo as mensagem 

```
kafka-console-producer --bootstrap-server localhost:9092  --topic alunos
>nome:fernando
```

Listando os consumer groups em outro terminal

```
docker exec -it kafka-broker /bin/bash
kafka-consumer-groups --bootstrap-server localhost:9092 --list
```

As configurações do consume groups são :

```
kafka-consumer-groups --bootstrap-server localhost:9092 --describe --group aplicacao-lab

```

Cancelando o consumidor e continuando produzindo mensgens

```
//Veja a descrição dos consumidores sem ter consumindo as mensagem
kafka-consumer-groups --bootstrap-server localhost:9092 --describe --group aplicacao-lab

```

Resentado o Offset Para o início (Voltando a posição inicial)

```
kafka-consumer-groups --bootstrap-server localhost:9092 --group aplicacao-lab --topic alunos --reset-offsets --to-earliest --execute

kafka-consumer-groups --bootstrap-server localhost:9092 --describe --group aplicacao-lab

```

Resentado o Offset Para o Final (Voltando a posição Final)

```
kafka-consumer-groups --bootstrap-server localhost:9092 --group aplicacao-lab --topic alunos --reset-offsets --to-latest --execute

kafka-consumer-groups --bootstrap-server localhost:9092 --describe --group aplicacao-lab

```


Para uma posição Específica

```
kafka-consumer-groups --bootstrap-server localhost:9092 --group aplicacao-lab --topic alunos --reset-offsets --to-offset 4 --execute

kafka-consumer-groups --bootstrap-server localhost:9092 --describe --group aplicacao-lab

```


Deletando os consumer groups

```
kafka-consumer-groups --bootstrap-server kafka:29092 --delete --group aplicacao-lab
```

Produzindo mensagem com a instrução Round Robin Partitioner

```
kafka-console-producer --bootstrap-server localhost:9092 --producer-property partitioner.class=org.apache.kafka.clients.producer.RoundRobinPartitioner --topic alunos
```
---

## Praticando mais - Desafio

![Cluster Mongo db](img/desafio.png)


O desafio terá a estrutura da imagem acima:

- Um tópico com nome preco-alterado com 3 partições
- Um consumer group com 3 consumidores


> Crie o tópico  e com a opção `RoundRobinPartitioner` para produizar as mensagens em cada consumidor



# Remover os containers

```
exit
docker-compose down
```
