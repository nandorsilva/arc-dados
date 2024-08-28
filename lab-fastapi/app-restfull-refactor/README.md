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

## Padrões Restfull, deixando mais profissional


Vamos criar mais arquivos

 ### Linux
```bash
#Testes
mkdir tests
touch tests/{__init__,conftest,test_api}.py

#Aplicação
mkdir -p app/{models,routes}
touch app/models/{aluno}.py
touch app/routes/{__init__,aluno}.py

#DockerCompose
touch docker-compose.yaml
```

 ### Powershell
```powershell
mkdir tests


$null | Out-File -FilePath "tests/conftest.py" -Encoding utf8
$null | Out-File -FilePath "tests/test_api.py" -Encoding utf8

mkdir app/models
mkdir app/routes

$null | Out-File -FilePath "app/models/aluno.py" -Encoding utf8
$null | Out-File -FilePath "app/routes/__init__.py" -Encoding utf8
$null | Out-File -FilePath "app/routes/aluno.py" -Encoding utf8


$null | Out-File -FilePath "docker-compose.yaml" -Encoding utf8


```


### Editaremos o arquivo `requirements.txt` e adicionaremos mais bibliotecas


```plain
fastapi[standard]
pydantic
pytest          # execução de testes
httpx
```

### Editaremos o arquivo `app/main.py` e adicionaremos mais bibliotecas

```python
from fastapi import FastAPI

from .routes import main_router

app = FastAPI(
    title="Fast Api Fia",
    version="0.1.0",
    description="Minha api",
)

app.include_router(main_router)

```

### Editaremos o arquivo `models/aluno.py` 


```python
from pydantic import BaseModel, EmailStr
from typing import  Optional

class AlunoResponse(BaseModel):
  id: int
  nome: str 
  idade: Optional[int] = None
  email: EmailStr

class AlunoRequest(BaseModel):
  id: int
  nome: str 
  idade: Optional[int] = None
  email: EmailStr
  senha: str


```
### Editaremos o arquivo `routes/aluno.py` 



```python
from fastapi import FastAPI,HTTPException, status, APIRouter
from typing import Annotated, Any, List, Optional
from pathlib import Path
from app.models.aluno import AlunoResponse, AlunoRequest



ALUNOS = [
    {"id": 1, "nome": "Pedro", "idade": 10, "email": "pedro@gmail.com", "senha": "1234"},
    {"id": 2, "nome": "Paulo", "idade": 20, "email": "paulo@gmail.com", "senha": "1234"},
    {"id": 3, "nome": "Gabriel", "idade": 35, "email": "gabriel@gmail.com", "senha": "1234"},
    {"id": 4, "nome": "Maria", "idade": 18, "email":"maria@gmail.com", "senha": "1234"}
]

router = APIRouter()


@router.get("/")
async def home():
    return "Olá alunos"


@router.get("/alunos/", response_model=List[AlunoResponse], status_code=status.HTTP_200_OK)
async def Alunos()-> Any:
    return ALUNOS


@router.get("/alunos/{idAluno}", response_model=AlunoResponse, status_code=status.HTTP_200_OK)
async def Consultar_Aluno(idAluno: Annotated[int, Path(title="O ID do aluno para a consulta", ge=1)]) -> Any:
      for item in ALUNOS:
        if item["id"] == idAluno:
            return item
      raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")
      

@router.post("/alunos", response_model=AlunoResponse, status_code=status.HTTP_201_CREATED)
# async def Inserir_Aluno(aluno: str):
async def Inserir_Aluno(aluno: AlunoRequest)-> Any:
  for _aluno in ALUNOS:
        if _aluno["id"] == aluno.id:
            raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST, detail="Já existe um aluno com esse ID")
        
  ALUNOS.append(aluno.dict())
 
  return aluno
  

@router.put("/alunos/{idAluno}", response_model=AlunoResponse,  status_code=status.HTTP_200_OK)
async def Atualizar_Aluno(idAluno:int, aluno: AlunoRequest) -> Any:
 for _aluno in ALUNOS:
        if _aluno["id"] == aluno.id:
            _aluno["nome"] = aluno.nome
            _aluno["idade"] = aluno.idade
            _aluno["email"] = aluno.email
            _aluno["senha"] = aluno.senha
            
            return _aluno
 raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")


@router.delete("/alunos/{idAluno}", response_model=AlunoResponse, status_code=status.HTTP_200_OK)
async def Deletar_Aluno(idAluno: int) -> Any:
    for index, alnno in enumerate(ALUNOS):
        if alnno["id"] == idAluno:
            return ALUNOS.pop(index)
    raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")

```
### Editaremos o arquivo `routes/__init__.py` 

```python
from fastapi import APIRouter

from .aluno import router as aluno_router

main_router = APIRouter()

main_router.include_router(aluno_router, prefix="/aluno", tags=["aluno"])


```

### Vamos editar o arquivo `tests/conftest.py`

```python

import pytest
from fastapi.testclient import TestClient

from  app.main import app


@pytest.fixture(scope="function")
def api_client():
    return TestClient(app)

```

### Vamos editar o arquivo `tests/test_api.py`

```python
import pytest

@pytest.mark.order(1)
def test_get_aluno(api_client):
    response = api_client.get("/aluno/")
    assert response.status_code == 200
    assert response.json() == "Olá alunos"

```

### Vamos editar o arquivo `Dockerfile`
```docker
FROM python:3.10

 
# Create the home directory
ENV APP_HOME=/home/app/api
RUN mkdir -p $APP_HOME
WORKDIR $APP_HOME
# 

# install
COPY . $APP_HOME
RUN pip install --no-cache-dir --upgrade -r requirements.txt
RUN pip install -e .


# 
CMD ["uvicorn","app.main:app","--host=0.0.0.0","--port=8000","--reload"]



```


### Vamos editar o arquivo `docker-compose.yaml`

```yaml
version: '3.9'

services:
  api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "8000:8000"   
    container_name:  fast-api-fia 
    volumes:
      - .:/home/app/api   
    stdin_open: true
    tty: true 
```

Atualizando a imagem pelo arquivo DockerCompose

```bash

docker container rm  fast-api-fia -f

docker compose up -d

docker image ls

docker logs  fast-api-fia

```

> [!IMPORTANT]
> A API vai ser atualizada automaticamente quando detectar mudanças no có



### Executando os Testes

```
docker compose exec api pytest tests/
```

4. [FastAPI e Esteira GitAction](../app-gitaction/README.md)
5. [FastAPI e Postgresql](../app-crud-db/README.md)