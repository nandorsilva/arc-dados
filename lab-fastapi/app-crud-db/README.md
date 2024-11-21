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
* Python 3.9
---


Vamos criar mais arquivos

 ### Linux
```bash

touch app/configs.py
touch app/db.py

mkdir postgres
touch postgres/create-databases.sh
touch postgres/Dockerfile

```

 ### Powershell
```powershell

$null | Out-File -FilePath "app/configs.py" -Encoding utf8
$null | Out-File -FilePath "app/db.py" -Encoding utf8

mkdir postgres

$null | Out-File -FilePath "postgres/create-databases.sh" -Encoding utf8
$null | Out-File -FilePath "postgres/Dockerfile" -Encoding utf8

```


### Editando o arquivo `requirements.txt` e adicionaremos mais bibliotecas


```plain
fastapi[standard]==0.115.4
pydantic
sqlmodel==0.0.8
asyncpg==0.28.0
uvicorn==0.22.0

# dev
pytest          # execução de testes
httpx
```

### Editando o arquivo `app/main.py` e adicionaremos mais bibliotecas

```python
from fastapi import FastAPI
from .configs import settings
from .db import init_db

from .routes import main_router

app = FastAPI(
    title="Fast Api Fia",
    version="0.1.0",
    description="Minha api",
)


@app.on_event("startup")
async def on_startup():
    await init_db()


app.include_router(main_router, prefix=settings.API_V1_STR)

```

### Editando o arquivo `models/aluno.py` 


```python
from typing import  Optional
from sqlmodel import Field, SQLModel


class AlunoBase(SQLModel):
    nome: str
    idade: Optional[int] = None  
    email: str = Field(max_length=50)
    senha: str



class Aluno(AlunoBase, table=True):
    __tablename__: str = 'Alunos'  
    id: int = Field(default=None, primary_key=True)

class AlunoRequest(AlunoBase):
  pass

class AlunoResponse(SQLModel):
  id: int
  nome: str 
  idade: Optional[int] = None
  email: str



```

### Editando o arquivo `routes/aluno.py` 



```python
from fastapi import HTTPException, status, APIRouter
from typing import Any, List
from app.models.aluno import Aluno, AlunoResponse, AlunoRequest
from sqlmodel import select, Session
from app.db import ActiveSession

router = APIRouter()


@router.get("/")
async def home():
    return "Olá alunos"


@router.get("/alunos/", response_model=List[Aluno], status_code=status.HTTP_200_OK)
async def Consultar_Alunos_ID(session: Session = ActiveSession)-> Any:
    result = await session.execute(select(Aluno))
    _alunos = result.scalars().all()
    return _alunos


@router.get("/alunos/{idAluno}", response_model=AlunoResponse, status_code=status.HTTP_200_OK)
async def Consultar_Aluno(idAluno:int,  session: Session = ActiveSession) -> Any:
     result = await session.execute(select(Aluno).where(Aluno.id == idAluno))
     _aluno = result.scalar_one_or_none()
     if _aluno is None:
         raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")
     return _aluno
     
       

@router.post("/alunos", response_model=AlunoResponse, status_code=status.HTTP_201_CREATED)
# async def Inserir_Aluno(aluno: str):
async def Inserir_Aluno(aluno: AlunoRequest, session: Session = ActiveSession)-> Any:
  db_aluno = Aluno.from_orm(aluno)
  session.add(db_aluno)
  await session.commit()
  await session.refresh(db_aluno)
  return db_aluno

@router.put("/alunos/{idAluno}", response_model=AlunoResponse,  status_code=status.HTTP_200_OK)
async def Atualizar_Aluno(idAluno:int, aluno: AlunoRequest, session: Session = ActiveSession) -> Any:
    result = await session.execute(select(Aluno).where(Aluno.id == idAluno))
    db_aluno = result.scalar_one_or_none()

    if db_aluno is None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")
    
    db_aluno.nome = aluno.nome
    db_aluno.email = aluno.email
    db_aluno.idade = aluno.idade
    db_aluno.senha = aluno.senha

    await session.commit()
    await session.refresh(db_aluno)
    return db_aluno


@router.delete("/alunos/{idAluno}", response_model=AlunoResponse, status_code=status.HTTP_200_OK)
async def Deletar_Aluno(idAluno: int, session: Session = ActiveSession) -> Any:   
    result = await session.execute(select(Aluno).where(Aluno.id == idAluno))
    db_aluno = result.scalar_one_or_none()
    if db_aluno is None:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")
    await session.delete(db_aluno)
    await session.commit()
    return db_aluno

```

### Editando o arquivo `postgres/create-databases.sh`
```bash
#!/bin/bash

set -e
set -u

function create_user_and_database() {
	local database=$1
	echo "Creating user and database '$database'"
	psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
	    CREATE USER $database PASSWORD '$database';
	    CREATE DATABASE $database;
	    GRANT ALL PRIVILEGES ON DATABASE $database TO $database;
EOSQL
}

if [ -n "$POSTGRES_DBS" ]; then
	echo "Creating DB(s): $POSTGRES_DBS"
	for db in $(echo $POSTGRES_DBS | tr ',' ' '); do
		create_user_and_database $db
	done
	echo "Multiple databases created"
fi

```

### Editando o arquivo `postgres/Dockerfile`
```yaml

FROM postgres:alpine3.14
# Uncomment to use Mac M1
# FROM --platform=linux/amd64 postgres:alpine3.14 
COPY create-databases.sh /docker-entrypoint-initdb.d/


```
### Editando o arquivo `docker-compose.yaml`

```yaml
version: '3.9'

services:
  api:
    image: fia_fastapi
    build:
      context: .      
      dockerfile: Dockerfile
    ports:
      - "8000:8000"   
    container_name:  fast-api-fia 
    volumes:
      - .:/home/app/api   
    environment:
      - DATABASE_URL=postgresql+asyncpg://postgres:postgres@db-postgres-fastapi:5432/dbfiafastapi
    stdin_open: true
    tty: true 

  db-postgres-fastapi:
    build: postgres
    image: fia_postgres_alpine3.14 
    container_name: postgres 
    ports:   
      - "5435:5432"
    environment:
      - POSTGRES_DBS=dbfiafastapi
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres

  pgadmin:
    container_name: pgadmin_container
    image: dpage/pgadmin4
    environment:
      PGADMIN_DEFAULT_EMAIL: lab-pgadmin4@pgadmin.org
      PGADMIN_DEFAULT_PASSWORD: postgres    
    ports:
      - "5433:80"
    depends_on:
      - db-postgres-fastapi    


```

### Editando o arquivo `app/configs.py`

```python
from pydantic import BaseSettings
import os

class Settings(BaseSettings):
    API_V1_STR: str = '/api/v1'
    DB_URL: str =  os.environ.get("DATABASE_URL")
    ##DB_URL: str = 'postgresql+asyncpg://postgres:postgres@db-postgres-fastapi:5432/dbfiafastapi'  


settings: Settings = Settings()


```

### Editando o arquivo `app/db.py`

```python
from sqlalchemy.orm import sessionmaker
from sqlalchemy.ext.asyncio import create_async_engine
from sqlalchemy.ext.asyncio import AsyncEngine
from sqlalchemy.ext.asyncio import AsyncSession
from fastapi import Depends
from sqlmodel import SQLModel, Session


from .configs import settings

engine: AsyncEngine = create_async_engine(settings.DB_URL, echo=True, future=True)

Session= sessionmaker(engine, class_=AsyncSession, expire_on_commit=False)


async def get_session() -> AsyncSession:
    async with Session() as session:
        yield session


async def init_db():
    print('Criando as tabelas no banco de dados...')
    async with engine.begin() as conn:
        #await conn.run_sync(SQLModel.metadata.drop_all)
        await conn.run_sync(SQLModel.metadata.create_all)
    print('Tabelas criadas com sucesso...')


ActiveSession = Depends(get_session)


```

### SQLAlchemy
ORM, ou Object-Relational Mapping (Mapeamento Objeto-Relacional), é uma técnica de programação que permite que você interaja com um banco de dados relacional usando uma abordagem orientada a objetos. Em vez de escrever diretamente consultas SQL para interagir com o banco de dados, você pode usar classes e objetos em sua linguagem de programação preferida.


Atualizando as imagens pelo arquivo DockerCompose

```bash 
docker compose up -d  db-postgres-fastapi pgadmin

docker image ls

```

### Configurando o acesso PgAdmin ao postgress



Acesso para o PgAdmin http://localhost:5433/


* Login: lab-pgadmin4@pgadmin.org
* Senha : postgres    

* Nome do server: postgres
* Nome do Host Name: postgres
* database: postgres
* Username: postgres
* password: postgres

### Tela de login do PgAdmin
![Exemplo Kafka Conect](../../content/login-pgadmin.png)


### Inserindo um server
![Exemplo Kafka Conect](../../content/add-server.png)

### Configurando o server
![Exemplo Kafka Conect](../../content/conect-pgadmin.png)



```bash 

docker container rm  fast-api-fia -f

docker-compose down api && docker-compose build api --no-cache && docker-compose up -d api

docker image ls

docker logs  fast-api-fia 

```

* http://localhost:8000/docs