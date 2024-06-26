
# LAB MYSQL

---
## Disclaimer
> **Esta configuração é puramente para fins de desenvolvimento local e estudos**
> 

---


## Pré-requisitos?
* Docker
* Docker-Compose


## Criando um container com a imagem MySql
```
docker run -d  -p 3306:3306 --name meubanco -e MYSQL_ROOT_PASSWORD=1234 mysql
```

Verificando se os containers foram criados com sucesso

```
 docker container ls
```
Verificando as imagens que foram feitas download do docker-hub
```
 docker image ls
```


Vamos executar alguns comandos de dentro do container meubanco

Acessar o Shell do container meubanco

```
docker exec -it meubanco bash
```

Conectando ao banco mysql
```
mysql -uroot -p
```

Digitar a senha que está na váriavel MYSQL_ROOT_PASSWORD


Listar os banco de dados existentes

```
show databases;
```

Criando um banco de dados


```
CREATE DATABASE arquitetura;

```

Acessar o banco de dados criado
```
USE arquitetura;
```


Exibir as tabelas do banco de dados `arquitetura`


```
SHOW TABLES;
```

## Criando tabelas

```
CREATE TABLE Cliente (id INT AUTO_INCREMENT PRIMARY KEY, nome VARCHAR(50), telefone VARCHAR(20), email VARCHAR(50), genero CHAR(1), estadocivil VARCHAR(20), dataNascimento DATE, uf char(2), dataCriacao DATE, dataAlteracao DATE);
```

```
CREATE TABLE Produto (id INT AUTO_INCREMENT PRIMARY KEY, nome VARCHAR(50), dataCriacao DATE);
SHOW TABLES;
```


Criar tabela do resultado de uma consulta

```


INSERT INTO Cliente values ('1','Teste da Silva','5511971111119','testedasilva@gmail.com','f','casado', '2000-03-31', 'SP', '2022-09-04','2023-06-17');

CREATE TABLE ClientesUF AS
SELECT *
FROM Cliente
WHERE uf = 'SP';

SELECT * FROM ClientesUF;

```

Alterado informações da tabela

```
ALTER TABLE Produto ADD Descricao varchar(255);

ALTER TABLE Produto RENAME COLUMN Descricao TO DescricaoProduto;

ALTER TABLE Produto MODIFY COLUMN DescricaoProduto varchar(300);

DESCRIBE Produto;

```

Crie um registro para a tabela Produto



Criado tabelas com relacionamento

```
CREATE TABLE Pedido (id INT AUTO_INCREMENT PRIMARY KEY, 
idCliente int, dataCriacao DATE,
constraint pedido_cliente foreign key (idCliente) references Cliente(id));
```

```
CREATE TABLE PedidoDetalhes (id INT AUTO_INCREMENT PRIMARY KEY, 
idPedido int,
idProduto int,
constraint pedido_detalhes_pedido foreign key (idPedido) references Pedido(id));
```

Outras formas de criar relacionamentos

```
ALTER TABLE PedidoDetalhes ADD CONSTRAINT pedido_detalhes_produto FOREIGN KEY(idProduto) REFERENCES Produto(ID);
```


## Inserindo dados nas tabelas



Informações de cliente - forma abreviada mas todos os campos

```
INSERT INTO Cliente values ('2','Teste de Souza','5511971111118','testedasouza@gmail.com','f','solteiro','1999-03-23','RJ','2023-06-17','2023-06-17');
INSERT INTO Cliente values ('3','Fernando Silva','5511971111117','fernandosilva@gmail.com','f','solteiro','1999-03-05','SP','2023-06-17','2023-06-17');
INSERT INTO Cliente values ('4','Fernando Souza','5511971111116','fernadosouza@gmail.com','f','solteiro','985-03-01','SP','2023-06-17','2023-06-17');
INSERT INTO Cliente values ('5','Virgulino da Silva','5511971111115','avast@gmail.com','f','solteiro','1999-03-30','RJ','2023-06-17','2023-06-17');
```


Informações do Produto 

```
INSERT INTO Produto (nome, dataCriacao) values ('Celular', CURDATE());
INSERT INTO Produto (nome, dataCriacao) values ('Computador', CURDATE());
select * from Produto;
```

Insert errado

```
INSERT INTO Produto values ('Computador');
```

Insert com select

```
//Criando um tabela copia de Produto


CREATE TABLE ProdutoCopia (id INT AUTO_INCREMENT PRIMARY KEY, nome VARCHAR(50), dataCriacao DATE, DescricaoProduto varchar(300));
```


Opção 1 - Insert com select

```
INSERT INTO ProdutoCopia
SELECT id, nome, dataCriacao, DescricaoProduto FROM Produto
WHERE nome='Celular';
```

Opção 2 - Insert com select

```
INSERT INTO ProdutoCopia (nome, DescricaoProduto)
SELECT nome, DescricaoProduto FROM Produto
WHERE nome='Computador';
```



## Atualizando dados nas tabelas


Atualiza todos os clientes

```
UPDATE Cliente SET nome = 'Nome alterado';
select * from Cliente;

```

Atualizar somente um cliente

```
UPDATE Cliente SET nome = 'Fernandinho Silva' WHERE telefone = 5511971111119;
select * from Cliente;
```

Atualizando os atributos nome e email

```
UPDATE Cliente SET nome = 'Fernandinho Silva', email='email@email.com.br' WHERE telefone = 5511971111119;
```

Update com select - Atualizar o cliente do pedido 1

```
//Fazendo a inserção primeiro
INSERT INTO Pedido (idCliente, dataCriacao) values(1, CURDATE());


UPDATE Cliente
INNER JOIN Pedido  ON Cliente.id =Pedido.idCliente
SET Cliente.Nome = 'Cliente do pedido'
WHERE Pedido.id = 1;
```



## Filtrando dados nas tabelas

```
select * from Cliente;

select * from Cliente where nome ='Teste da Silva';

select * from Cliente where telefone = '5511971111119' and email ='email@email.com.br';

select * from Cliente where telefone = '5511971111119' or telefone ='5511971111114';


select nome, telefone, email from Cliente where telefone = '5511971111119';   
```

### Um pouco de joins

```
select * from Cliente c
left outer join Pedido p on c.id = p.idCliente;
```

Fazer um join mostrando somente os campos de cliente

```
select c.* from Cliente c
left outer join Pedido p on c.id = p.idCliente;

select c.* from Cliente c
inner join Pedido p on c.id = p.idCliente;

select c.* from Cliente c
right outer join Pedido p on c.id = p.idCliente
where c.telefone = '5511971111119';

select c.* from Cliente c
inner join Pedido p on c.id = p.idCliente
inner join PedidoDetalhes pd on p.id = pd.idPedido;
```

Quantos clientes existem na tabela

```
select count(1) from Cliente;
```

Tirando as duplicidades na consulta

```
SELECT DISTINCT nome  FROM Cliente;
```

Agrupando os dados pelo campo estados

```
SELECT UF, Count(1) as Total FROM Cliente
Group by UF;
```


Agrupando os dados de estados maior que 1

```
SELECT UF, Count(1) as Total FROM Cliente
Group by UF
having count(*) >1;
```


Ordenando os dados  ASC|DESC

```
SELECT * FROM Cliente 
ORDER BY telefone ASC;
```

Consultando os dados entre as datas

```
SELECT *
FROM Cliente
WHERE dataCriacao BETWEEN '2022-09-01' AND '2022-09-04';
```


Customizando um atributo com case

```
SELECT nome,
CASE
    WHEN uf ='SP'  THEN 'São Paulo'    
    ELSE uf
END AS estado
FROM Cliente;
```

## Removendo dados nas tabelas

```
delete from Cliente where id = 1 ;

delete from Pedido where id = 1 ;
```

# Transformar registros em json

```
SET @jsonempl=(SELECT JSON_ARRAYAGG(JSON_OBJECT("id", id, "nome", nome, "telefone", telefone, "email", email)) FROM Cliente);

SELECT JSON_PRETTY(@jsonempl);

```


## Criando indíces


```
CREATE INDEX idx_telefone ON Cliente (Telefone);

```


# Instalando um ferramenta gráfica para o MySQL

https://dev.mysql.com/downloads/workbench/


## Caso de Uso
A LoSil é uma empresa fictícia que atua no ramo de comércio eletrônico, oferecendo uma ampla variedade de produtos para seus clientes. Com o crescimento do negócio e a necessidade de gerenciar pedidos, ordens e informações dos clientes de forma eficiente, a LoSil decidiu adotar um banco de dados Relacional, como o MySQL.

Após uma análise detalhada das necessidades da empresa, a equipe de desenvolvimento definiu as seguintes entidades principais para a modelagem do banco de dados:


Clientes:

* ID do cliente
* Nome
* Endereço
* Informações de contato (e-mail, telefone, etc.)

Pedidos:

* ID do pedido
* Data do pedido
* Status do pedido (pendente, em processamento, concluído, cancelado, etc.)
* ID do cliente associado

Itens do pedido:

* ID do item do pedido
* ID do pedido associado
* ID do produto
* Quantidade

Produtos:

* ID do produto
* Nome do produto
* Descrição
* Preço
* Estoque disponível

Estoque:

* ID do Estoque
* ID do produto
* Quantidade


Com base nessas entidades, a equipe de desenvolvimento projetou as seguintes tabelas:


# Remover os containers

```
exit
docker-compose down
```
