# Lab Eda - Connect

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


## Kafka Connect

![Exemplo Kafka Conect](../content/kafka-connect.png)

### Realizando download dos plugins Debezium Sql Server e PostGree (Source) e Mongodb (Sink)

```

cd kafka-connect

mkdir plugin
wget https://repo1.maven.org/maven2/org/mongodb/kafka/mongo-kafka-connect/1.6.1/mongo-kafka-connect-1.6.1-all.jar -P plugin

curl https://repo1.maven.org/maven2/io/debezium/debezium-connector-sqlserver/1.6.2.Final/debezium-connector-sqlserver-1.6.2.Final-plugin.tar.gz | tar xvz -C plugin

curl -sfSL https://repo1.maven.org/maven2/io/debezium/debezium-connector-postgres/2.1.3.Final/debezium-connector-postgres-2.1.3.Final-plugin.tar.gz | tar xz -C plugin

```

Criando a imagem com os plugins realizados o downloads


```
 docker image build -t fernandos/kafka-connet-debezium-lab  -f Dockerfile .
 
```

Vamos enviar a imagem para o dockerhub ??

```
docker image push <<conta>>/kafka-connet-debezium-lab
```

> As imagens customizadas encontra-se no https://hub.docker.com/

### Subindo um cluster kafka connect

Subindo o cluster kafka com Jaeger

```
docker-compose up -d connect jaeger
```

Container  criado? Vamos ver!

```
docker container ls
```

Listando os plugins existente, os defaults da imagem e os debezium que foi inserido na imagem, via Dockerfile

```
docker exec -it kafkaConect curl  http://localhost:8083/connector-plugins
```

### Configurando Banco de dados CDC para SQL

Será utilizado o connector debezium para sql server, ele faz a leitura do banco de dados via CDC.

> Para nossa exemplo iremos subir um banco de dados, caso já tenha um banco habilitado o CDC pode-se usar ele. Mais detalhes do que é Sql Server CDC, https://docs.microsoft.com/pt-br/sql/relational-databases/track-changes/about-change-data-capture-sql-server?view=sql-server-ver15

```
docker-compose up -d sqlserver
```

Para esse tutorial estou utilizando a imagem sql server da Microsoft mcr.microsoft.com/mssql/server:2019-latest. Para criar a estrutura dos dados estou utilizando o próprio container criado.

>O arquivo para habilitar CDC e criar o banco de dados, as tabelas e popular com alguns dados está em sql/init.sql que foi executado via Microsoft SQL Server Management Studio ou você pode executar pelo próprio pod conforme código abaixo

Executando o script

```
cat sql/init.sql | docker exec -i lab-eda-sqlserver-1 /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD

docker exec -i lab-eda-sqlserver-1 /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -d dbEcommerce -Q "select * from produtos"

docker exec -i lab-eda-sqlserver-1 /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -d dbEcommerce -Q "INSERT INTO produtos(nome,descricao)  VALUES ('Lapis','lapis de escrever');"

```

### Configurando pgAdmin

criar prints


### Criando os Conectores

API rest do kafka Connect
https://docs.confluent.io/platform/current/connect/references/restapi.html


Criando o conector PostGres

```
   http PUT http://localhost:8083/connectors/connector-postgres/config < conector-postgres.json
```

Criando o conector Sql Server


```
 http PUT http://localhost:8083/connectors/connector-sql/config < conector-sql.json
```


Listando os conectors

```
http http://localhost:8083/connectors/
```


Criando um produto ---rever texto

```
export SA_PASSWORD=Password!

docker exec -i lab-eda-sqlserver-1 /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -d dbEcommerce -Q "INSERT INTO produtos(nome,descricao)  VALUES ('Lapis','lapis de escrever');"

```

Listando os tópicos

//criar caminho para acesso --abrir um terminal e tal

```
docker exec -it kafka101 /bin/bash
kafka-topics --bootstrap-server localhost:9092 --list 
```


Consumindo mensagem sqldebezium.dbo.produtos - Datasource SQL Server

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic sqldebezium.dbo.produtos --from-beginning
```

Consumindo mensagem dbserver1.inventory.orders Datasource Postgres

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic dbserver1.inventory.orders --from-beginning
```


#### colocar comando da api, ver status do connetor e tals...






