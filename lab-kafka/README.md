
# LAB KAFKA

---
## Disclaimer
> **Esta configuração é puramente para fins de desenvolvimento local e estudos**
> 

---


## Pré-requisitos?
* Docker
* Docker-Compose


## Criando o ambiente Kafka com o docker compose
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

Verificando se a ferramenta Kafdrop subiu com sucesso
http://localhost:19000/




Vamos executar alguns comandos de dentro do container kafka1

Acessar o Shell do container kafka1

```
docker exec -it kafka1 /bin/bash
```

# Criando nosso Primeiro tópico
```
kafka-topics --bootstrap-server localhost:9092 --topic alunos --create
```

Listando os tópicos criado
```
kafka-topics --bootstrap-server localhost:9092 --list 
```

Alguém lembra das partições, agora o topico com mais de uma partição

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
...é não deu certo, porque ?

```
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos-factor --create --partitions 3 --replication-factor 1
```

# Deletando um tópico

```
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos-factor --delete
kafka-topics --bootstrap-server localhost:9092 --topic alunos-novos-factor --describe

```

# Produzinho mensagens

```
kafka-console-producer --bootstrap-server localhost:9092 --topic alunos

> Minha primeira mensagem
>Melhor lab do brasil
>Eu sou o Fulano
>^C  (<- Ctrl + C is used to exit the producer)

```

Produzinho mensagens com acks

```
kafka-console-producer --bootstrap-server localhost:9092 --topic alunos --producer-property acks=all
```

Criando um topico no momento de criar a mensagem

```
kafka-console-producer --bootstrap-server localhost:9092 --topic professor

kafka-topics --bootstrap-server localhost:9092 --topic professor --describe

```

A Criação foi possivel pois a propriedade auto.create.topics.enable está com true

O Topico foi criado com configurações default

Ver as configurações na pasta cat /etc/kafka/server.properties

Mudar o arquivo docker-compose


Novo topico com a configuração numero de partição ajustada

```

//Sair do container
exit

//Mudar o docker compose colocando
KAFKA_NUM_PARTITIONS=3

//deploy docker-compose
docker-compose up -d

//Entrar no conteiner
docker exec -it kafka1 /bin/bash

kafka-console-producer --bootstrap-server localhost:9092 --topic topico-conf

kafka-topics --bootstrap-server localhost:9092 --topic topico-conf --describe
```

Produzir mensagens com chaves

```
kafka-console-producer --bootstrap-server localhost:9092 --topic alunos --property parse.key=true --property key.separator=:
>key:value
>aluno:fernando
```

# Consumindo mensagens

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic alunos
```

Abri outro terminal, entre no container e produza uma mensagem

```
docker exec -it kafka1 /bin/bash

kafka-console-producer --bootstrap-server localhost:9092 --topic alunos --property parse.key=true --property key.separator=:

>aluno:fernando
>aluno:felipe

```

Consumindo as mensagens desde o inicio

No primeiro terminal cancela o consumo da mensagem

```
>^C  (<- Ctrl + C is used to exit the producer)

kafka-console-consumer --bootstrap-server localhost:9092 --topic alunos --from-beginning

```

Consumindo mensagens mostrado algumas configurações como a chave e valor

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic alunos  --property print.timestamp=true --property print.key=true --property print.value=true --property print.partition=true --from-beginning
```

# Consumer group





Cria um consumer group

Consumindo as mensagens com um consumer group

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic alunos --group aplicacao-lab
```

Abrir outro terminal 

Produzindo as mensagem 

```
kafka-console-producer --bootstrap-server localhost:9092  --topic alunos
>nome:fernando
```

Listando os consumer groups em outro terminal

```
docker exec -it kafka1 /bin/bash
kafka-consumer-groups --bootstrap-server localhost:9092 --list
```

As configurações do consume groups são :

```
kafka-consumer-groups --bootstrap-server localhost:9092 --describe --group aplicacao-lab

```

Produzindo mensagem no Round Robin Partitioner

```
kafka-console-producer.sh --bootstrap-server localhost:9092 --producer-property partitioner.class=org.apache.kafka.clients.producer.RoundRobinPartitioner --topic alunos
```

## Praticando mais

Crie o ambiente

Crie um topico com 3 partições
Produce com RoundRobinPartitioner
Crie 2 consumer group para o topico criado

# Remover os containers

```
exit
docker-compose down
```
