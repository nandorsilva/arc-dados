# Lab Eda - KSQLdb

---
## Disclaimer
> **As configurações dos Laboratórios é puramente para fins de desenvolvimento local e estudos**
> 

---

## Pré-requisitos?
* Docker
* Docker-Compose

# Instalação Kafka 

[LAB EDA](lab-eda//README.md)
---

# KSqldb

Entrando no container cli

```
cd ksqldb
docker-compose up -d 
docker-compose exec ksqldb-cli ksql http://ksqldb-server:8088
```


### Criando um tópico

Eu outro terminal crie um tópico
```
docker exec -it kafka /bin/bash
kafka-topics --bootstrap-server localhost:9092 --create --partitions 1 --replication-factor 1 --topic alunos ;
```

No terminal do KSQL execute o comando abaixo:

```
ksql> show topics;

ksql> list topics;
```

Terminal Linux

```
kafka-console-producer --bootstrap-server localhost:9092 --topic alunos --property parse.key=true --property key.separator=:

//key:value

>aluno:aluno 1
>aluno:aluno 2
>aluno:aluno 3
>aluno:aluno 4
```

No terminal KSQLdb

```
print 'alunos' from beginning;

print 'alunos' from beginning limit 2;

print 'alunos' from beginning interval 2 limit 2 ;

print 'alunos' from beginning interval 2 ;

```


### Criando nosso primeiro stream

> No terminal do KSQLdb

```
create stream alunos_stream (id int, nome varchar , curso varchar) with (kafka_topic='alunos', value_format='DELIMITED');

```


Listando o Stream

```
list streams;
```


Descrever o Stream

```
describe ALUNOS_STREAM;

```

Selecionando os dados

```
select rowtime, id, nome, curso from ALUNOS_STREAM emit changes;
```

No outro terminal - linuz




```

//Se não estiver dentro do container

docker exec -it kafka /bin/bash

kafka-console-producer --bootstrap-server localhost:9092 --topic alunos --property parse.key=true --property key.separator=:

>aluno:1,aluno 1 ,arquitetura de dados
>aluno:2,aluno 2 ,engenharia de dados
>aluno:3,aluno 3 ,engenharia de dados
>aluno:4,aluno 4 ,arquitetura de dados
```

Formatando a data da query no terminal KSQLdb


```
select FORMAT_TIMESTAMP(FROM_UNIXTIME(rowtime), 'yyyy-MM-dd HH:mm:ss') as data , id, nome, curso from ALUNOS_STREAM emit changes;

```

Cade os dados ??


```
 //Configuração para ver todas as mensagens produzidas
 SET 'auto.offset.reset'='earliest';
```

Agrupando as mensagens

```
^C
select curso, count(*) from ALUNOS_STREAM  group by curso emit changes;
```

Produzinho mais uma mensagem no outro terminal

```
>aluno:4,aluno 4 ,arquitetura de dados
```



```
^C
select FORMAT_TIMESTAMP(FROM_UNIXTIME(rowtime), 'yyyy-MM-dd HH:mm:ss') as data , id, nome, curso from ALUNOS_STREAM emit changes limit 4;
```

### Criando seu stream no formato json

Terminal linux

```
kafka-topics --bootstrap-server localhost:9092 --create --topic professores --partitions 1 --replication-factor 1
```

Terminal KSqlDB

```
list topics;

create stream professores_stream (id int, nome varchar , materia varchar, quantidadeaula int) with (kafka_topic='professores', value_format='json');

show streams;

select rowtime, id, nome from professores_stream emit changes;

```


Terminal linux

```
kafka-console-producer --bootstrap-server localhost:9092 --topic professores --property parse.key=true --property key.separator=:

>professor1:{"id":1, "nome":"Fernando", "materia":"dados" , "quantidadeaula": 2}
>professor2:{"id":2, "nome":"Fabio", "materia":"dados", "quantidadeaula": 4}
>professor3:{"id":3, "nome":"Felipe", "materia":"dados", "quantidadeaula": 6}
```

### Funções escalares


> https://docs.ksqldb.io/en/latest/developer-guide/ksqldb-reference/scalar-functions/

Terminal KsqlDB


```
select FORMAT_TIMESTAMP(FROM_UNIXTIME(rowtime), 'yyyy-MM-dd HH:mm:ss', 'America/Sao_Paulo') as data, id, nome from ALUNOS_STREAM emit changes;



select CAST(FROM_UNIXTIME(rowtimE) AS DATE) as data, id, nome from ALUNOS_STREAM emit changes;


select CAST(FROM_UNIXTIME(rowtimE) AS DATE) as data, id, ucase(nome) as nome from ALUNOS_STREAM emit changes;


```


---

### Visões Stream


Criando uma consulta baseada em um stream.

Terminal Ksqldb

```

SET 'auto.offset.reset'='earliest';


select  id, 'Professor: ' + ucase(nome) + ' de ' + materia 
+ ' , a quantidade de matéria é: '
+ case when quantidadeaula <= 2  then 'Bom'
       when quantidadeaula between 3 and 5 then 'Ótimo'
       else 'Excelente' 
   end as descricao
from professores_stream;

```

Executando o script criando a view.

```
run script '/scripts/view_professores_stream.ksql';

show streams;


```

No terminal do Linux crie mais uma mensagem


```
professor4:{"id":4, "nome":"Maria", "materia":"dados", "quantidadeaula": 8}
```

No terminal Ksqlsb


```
 describe view_professores_stream extended;

select descricao from view_professores_stream emit changes;
  
```

No terminal do Linux crie mais uma mensagem


```
professor5:{"id":5, "nome":"Maria", "materia":"dados", "quantidadeaula": 10}


```

Observe o tópico criado

```
kafka-topics --bootstrap-server localhost:9092 --list 
```

Aplicando um consumer no tópico `VIEW_PROFESSORES_STREAM`

```
kafka-console-consumer --bootstrap-server localhost:9092 --topic VIEW_PROFESSORES_STREAM --from-beginning
```

Apagando o stream 

```
 drop stream VIEW_PROFESSORES_STREAM;
```

---

## Tabelas


No terminal Linux vamos criar nosso topico para trabalhar com tabelas

```
kafka-topics --bootstrap-server localhost:9092 --create --partitions 1 --replication-factor 1 --topic produto ;

```

No terminal KSqldb criar a tabela `produtosTable`

```
create table produtosTable (idProduto int primary key, produto varchar) with (KAFKA_TOPIC='produto', KEY_FORMAT = 'JSON',
  VALUE_FORMAT = 'JSON' , PARTITIONS = 3);

show tables;

describe produtosTable;

describe  produtosTable extended;

select idProduto, produto from produtosTable emit changes;

```

No terminal Linux, vamos criar algumas mensagens

```
kafka-console-producer --bootstrap-server localhost:9092 --topic produto --property parse.key=true --property key.separator=:

>1:{"produto":"Celular"}
>2:{"produto":"TV"}
>3:{"produto":"Geladeira"}
>4:{"produto":"Maquina"}
```


No terminal Ksqldb

```
 select * from PRODUTOSTABLE where idproduto= 2  emit changes;
```

No terminal Linux, vamos criar a mensagem 2

```
>2:{"produto":"TV - Alterada"}

^C
kafka-consumer-groups --bootstrap-server localhost:9092 --list

//Verifica se tem o topico *PRODUTOSTABLE*


 kafka-console-consumer --bootstrap-server localhost:9092 --topic _confluent-ksql-default_transient_transient_PRODUTOSTABLE_164564820911461907_1691341622923 --property print.timestamp=true --property print.key=true --property print.value=true --property print.partition=true --from-beginning


//Listando as configurações do topico 

kafka-topics --bootstrap-server 127.0.0.1:9092 --describe --topic _confluent-ksql-default_transient_transient_PRODUTOSTABLE4_2388878536066455653_1689963843486-KsqlTopic-Reduce-changelog --describe


drop table IF EXISTS produtosTable DELETE TOPIC;

```


### Joins Ksqldb

> Stream podem fazer join com Stream
  Stream com Stream gera um novo stream
  Table com Table gera uma nova table
  Stream com Table gera um novo Stream


Criando um topico para preço no terminal Linux


```
kafka-topics --bootstrap-server localhost:9092 --create --topic produto-preco --partitions 1 --replication-factor 1

```

Criando o stream para os preços dos produto no terminal Ksqldb


```
create stream produto_preco_stream (idprodutopreco int, idproduto int,  preco decimal(18,2)) with (kafka_topic='produto-preco', value_format='json');

```

Criando Join Stream x Table

```
select p.idproduto, p.PRODUTO, pp.preco
from produto_preco_stream pp 
left join PRODUTOSTABLE p on pp.idproduto=p.idproduto emit changes;
```

Criando mensagens no topico de preço no terminal Linux

```
kafka-console-producer --bootstrap-server localhost:9092 --topic produto-preco --property parse.key=true --property key.separator=:

>1:{"idprodutopreco":1,"idproduto":1, "preco": 1.99}
>2:{"idprodutopreco":2,"idproduto":2, "preco":10}
>3:{"idprodutopreco":3,"idproduto":3, "preco":1500}

```

Invertendo o join...Terminal Ksqldb
```
select pp.idproduto, pp.PRODUTO, p.preco
from PRODUTOSTABLE  pp 
left join produto_preco_stream p on pp.idproduto=p.idproduto emit changes;
```
> Opa!!!!!!!!!
>Invalid join order: table-stream joins are not supported; only stream-table joins.



Criando um um strema do join

```
create stream produto_com_precos as 
select p.idproduto, p.PRODUTO, pp.preco
from produto_preco_stream pp 
left join PRODUTOSTABLE p on pp.idproduto=p.idproduto emit changes;

select P_IDPRODUTO, produto , preco from produto_com_precos  emit changes;
```

4:{"idprodutopreco":5,"idproduto":1, "preco":1500}


--------

## Consultas Push e Pull

> Push emit changes
> Pull query, executa a consulta e acaba o processamento/consulta

### Inserção no Ksql


```

INSERT INTO produto_preco_stream (idprodutopreco, idproduto, preco) VALUES (5, 1, 2.99);
INSERT INTO produto_preco_stream (idprodutopreco, idproduto, preco) VALUES (5, 1, 2.99);


INSERT INTO PRODUTOSTABLE (idProduto, produto) VALUES (3, 'Computador');
INSERT INTO PRODUTOSTABLE (idProduto, produto) VALUES (4, 'Geladeira');
 ```


 ### Formatos Ksqldb

 * NONE
 * DELIMITED (csv)
 * JSON
 * AVRO
 * KAFKA
 * PROTOBUF



## Planos de execução

#Explain Plan

 ```
show queries;
explain (id-query) 

 ```

 >Visualizando o plano de execução
 https://zz85.github.io/kafka-streams-viz/