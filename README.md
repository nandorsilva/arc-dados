
# LAB MONGODB


## Disclaimer
> **Esta configuração é puramente para fins de desenvolvimento local e estudos**
> 

O Arquivo `docker-compose` provisiona cluster de mongodb com replica set com 3 instâncias: 
- mongo1:27017
- mongo2:27017
- mongo3:27017


## Pré-requisitos?
* Docker
* Composição do Docker

> O endereço configurado no host local.

* Linux /etc/hosts
* Windows C:\Windows\System32\drivers\etc

```
127.0.0.1       mongo1
127.0.0.1       mongo2
127.0.0.1       mongo3
```

## Executando réplica set Monogodb
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

Vamos executar alguns comandos de dentro do cluster mongo1 como configurar as tags

Acesso o Shell do container mongo1

```
docker exec -it mongo1 sh -c "mongo --port 30001"
```

Executando o script `scripts\rs-init.sh` para a criação do replica-set


Verificando os bancos de dados existentes
```
show dbs
```

Apontando um banco local do mongodb
```
use local
```

Lista as collections do banco local selecionado
```
show collections
```

Criando um documento simples

db.lab.insert({produto: "lapis", categoria: "papelaria"})

Criado as tags para organização das réplicas:

```
conf = rs.conf()
conf.members[0].tags = { "dc": "SP"}
conf.members[1].tags = { "dc": "SP"}
conf.members[2].tags = { "dc": "RIO" }
rs.reconfig(conf)
```