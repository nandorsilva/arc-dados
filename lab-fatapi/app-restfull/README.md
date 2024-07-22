# LAB Fast API
---
## Disclaimer
> **Esta configuração é puramente para fins de desenvolvimento local e estudos**
> 

---

## Pré-requisitos?
* Docker
* Docker-Compose
* Editor de códigos como VSCode, Sublime, Vim
* Python 3.10
---

## Criando um repositório

Acesse a página do github e clique em “Create Repository”

![Criando repositorio](../content/devops-01.png)


1. Coloque o nome do repositorio de sua preferencia;
2. Informe uma descrição para o repositório;
3. Configure o mesmo como privado ou público. > Um repositório privado só é acessivel
pelo seu usuário e os colaboradores do mesmo, o público tem seu código acessivel por
toda internet;
4. Clique em Create Repository.


![Criando repositorio](../content/devops-02.png)

1. [LAB Devops](../lab-devops/README.md)

## FastAPI

FastAPI é um framework web moderno e rápido (alto desempenho) para a construção de APIs com Python. Ele é desenhado para criar APIs da web de maneira rápida e fácil, com foco em alta performance, facilidade de uso e documentação automática.



### Principais características do FastAPI:

* Rápido: O desempenho do FastAPI é comparável ao do Node.js e do Go (graças ao Starlette e Pydantic).
* Fácil de usar: Minimiza o código repetitivo e facilita a criação de endpoints de API.
* Documentação automática: Gera documentação interativa automaticamente com Swagger UI e ReDoc.


## Ambiente

No nosso ambiente estamos usando o Docker para executar o FastAPI

## Nossa primeira API


Dentro da pasta app-helloworld possui o arquivo main.py, copie ele na para a estrutura


//criando a estrutura de pastas

```
mkdir app
touch app/main.py
touch Dockerfile
touch requirements.txt
```


### Criando estrutura do arquivo main.py


### Criando estrutura do requirements.txt


### Criando estrutura do arquivo Dockerfile

### Executando via Docker

```
docker image build -t fernandos/app-fastapi-fia .

docker container run -d --name fast-api-fia -p 80:80  fernandos/app-fastapi-fia

docker logs  fast-api-fia

```


//Swaager
http://localhost:8000/docs


http://localhost:8000/openapi.json


http://localhost:8000/redoc


### Recheando nosso projeto

Vamos agora abrir a pasta app-crud e criar os arquivos `docker-compose.yaml` `setup.py` e não esqueça de atualizar o arquivo `main.py`

> Copie o conteúdo no repositório lab-fastApiFia

![Estrutura pasta fastapi](content/lab-fastapi-01.png)

Depois de criado vamos executar docker compose, lembra como se faz??

```
Docker compose up -d
```

### Vamos analisar o arquivo `main.py` !?!?!


![Request/response fastapi](content/lab-fastapi-02.png)


### Vamos melhorar Nossa arquitetura ?

> Copie o conteúdo no repositório lab-fastApiFia

![Estrutura pasta fastapi](content/lab-fastapi-01.png)




Testes

```
docker compose exec api pytest tests/
```


### Criando estrutura do arquivo main.py

### Criando estrutura do requirements.txt


### Criando estrutura do arquivo Dockerfile


### Criar nossa imagem e o container





//Swaager
http://localhost:8000/docs


http://localhost:8000/openapi.json


http://localhost:8000/redoc


### Criar nosso docker-compose
