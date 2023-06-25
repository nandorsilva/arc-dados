
# LAB MONGODB

---
## Disclaimer
> **Esta configuração é puramente para fins de desenvolvimento local e estudos**
> 

---

 <img src="" width="200" />
![Cluster Mongo db](../img/cluster-mongdb.png)

O Arquivo `docker-compose` provisiona cluster de mongodb com replica set de 3 instâncias: 

- mongo1:27017
- mongo2:27017
- mongo3:27017

---

## Pré-requisitos?
* Docker
* Docker-Compose

> O endereço configurado no host local.

* Linux /etc/hosts
* Windows C:\Windows\System32\drivers\etc

---

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

## Configurando Replica-set

Executando o script `scripts\rs-init.sh` para a criação do replica-set

```
docker exec -it mongo1 /bin/bash
cd scripts
./rs-init.sh
```


Vamos executar alguns comandos de dentro do cluster mongo1 para configurar as tags

Acessar o Shell do container mongo1

```
//Se tiver fora do container
docker exec -it mongo1 sh -c "mongo --port 27017"
```
ou
```
//dentro do container
mongo --port 27017
```


Verificando os bancos de dados existentes
```
show dbs
```

Apontando ou criando um banco no mongodb
```
use dbcursofia
```

Criando um documento simples


```
db.lab.insert({produto: "lapis", categoria: "papelaria"})

```

Lista as collections do banco local selecionado
```
show collections
```


Outra forma de inserir um documento simples


```
db.getCollection("lab.")insert({produto: "lapis", categoria: "papelaria"})

```

Criado as tags para organização das réplicas:

```
conf = rs.conf()
conf.members[0].tags = { "dc": "SP"}
conf.members[1].tags = { "dc": "SP"}
conf.members[2].tags = { "dc": "RIO"}
rs.reconfig(conf)
```



Verificando status do server

```
db.serverStatus()
```

## Inserindo documentos

```
for (var i = 1; i <= 100; i++) db.lab.insert({produto: "produto:" + x , categoria: "papelaria"})
db.lab.find()
db.lab.count()
```

Inserindo sem o ID
```
db.produtos.insert( { Produto: "Celular", Preco: 10 } )
```

Inserindo com o ID (_id)
```
db.produtos.insert( {_id: 1,  Produto: "Celular", Preco: 10 } )
```

Inserindo multiplos documentos
```
db.produtos.insert(
   [
     { _id: 10, Produto: "TV", preco: 1.99 },
     { Produto: "Geladeira", preco: 5000 },
     { Produto: "Geladeira", preco: 3400 },
     { Produto: "Computador", preco: 2500 },
     { Produto: "Computador Melhor", preco: 4500 },
     {_id: 500, Produto: "Cama", preco: 100 }
   ]
)
```


## Consultando documentos

Busca todos os documentos
```
db.produtos.find()
```
Buscando com o operador `igual`
```
db.produtos.find( { Produto: "Geladeira" } )
```
Buscando com o operador `igual` com mais de um campo

```
db.produtos.find( { Produto: "Geladeira", preco:3400 } )
```

Buscando com o operador `Range`
```
db.produtos.find( { preco: { $gt: 1000, $lt: 5000 } } );
```

Alguns operadores

* $gt maior que (greater-than)
* $gte igual ou maior que (greater-than or equal to)
* $lt menor que (less-than)
* $lte igual ou menor que (less-than or equal to)
* $ne não igual (not equal)
* $in existe em uma lista
* $nin não existe em uma lista
* $all existe em todos elementos
* $not traz o oposto da condição


Buscando com o operador `Like`
```
db.produtos.find({"Produto":/Computador/});
```

Tipos de `Likes`
```
db.produtos.find({"Produto":/Computador/});
db.produtos.find({Produto: /^Melhor/}) // Like 'Melhor%'
db.produtos.find({Produto: /Ca$/}) // Like '%CA'
```


## Importando arquivos CSV (VALIDAR)

O Arquivo `mongo-import.sh`  vai ler os arquivos csv e importar para o mongodb utilizando a ferramenta `mongoimport`

https://www.mongodb.com/docs/database-tools/mongoimport/


Usando o comando `explain` para extrair informações importantes de uma consulta.

```
db.orders.find({"CustomerID" :"BONAP"}).explain();
```

## Collection Capped

Verifiando se a collection é do tipo Capped
```
db.orders.isCapped()
```

Convertendo uma collection para Capped

```
db.runCommand( { collMod: "produtos", cappedSize: 100000 } )
db.produtos.isCapped()
```

Criando uma collection

```
db.createCollection("colecaonova", { capped: true, autoIndexID : true, size: 6142800, max: 10000 })
```

## Criando índice

db.collection.ensureIndex(
{ <campo1> : <ordem>,
<campo2> : <ordem>,
...} );

```
db.orders.ensureIndex({ CustomerID : 1});
db.orders.find({"CustomerID" :"BONAP"}).explain();
```

Listar os índices criados
```
db.orders.getIndexes();
```

Remover um índice criado
```
db.orders.dropIndex("<<nome do indice>");
```

## Alterando documentos

Atualizando o documento todo
```
db.produtos.update( { _id: 10} , {Produto: "TV 30 polegadas", preco: 10.99 })
```

Atualizando uma entidade do documento
```
db.produtos.update({_id : 10}, {$set:{ "Produto": "TV 30 polegadas - Alterada" }})
```

Criando um atributo no documento
```
db.produtos.update({_id : 10}, {$inc:{ "Categoria": "Eletronico"}})
```

Atualizando vários documentos

```
db.produtos.update( { Produto: "Geladeira"} , { $set: { preco: 10000} })

```

## Foi possível ?

Precisamos habilitar a atualização para multiplos documentos

```
db.produtos.update( { Produto: "Geladeira"} , { $set: { preco: 10000}  }, { multi: true })
```

Se o documento não for encontrado ? 
Adiciona documento se update não tem o filtro existente

```
{ Produto: "Geladeira", preco: 5000 }

db.produtos.update(
   {_id : 101},
   { Produto: "Geladeira Nova", preco: 5000 } ,
   { upsert: true }
)
```


## Excluindo documentos

db.produtos.remove({_id: 101})

## Cluster

Verificando o status do cluster

```
rs.status();
```

Criando um usuário

```
db.createUser({user: 'admin', pwd: 'admin', roles: [ { role: 'root', db: 'admin' } ]});
```


Exibir operações rodando

```
db.currentOp();

```