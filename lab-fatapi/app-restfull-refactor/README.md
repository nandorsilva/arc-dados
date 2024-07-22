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

//criar um desenho com o modelos



### Editaremos o arquivo `requirements.txt` e adicionaremos mais bibliotecas


```plain
fastapi
pydantic
pytest          # execução de testes
httpx
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

Atualizando a imagem pelo arquivo DockerCompose

```bash
docker compose up -d

```


Execute o container para testar a aplicação

```console

docker container run -d --name fast-api-fia -p 80:80  <<seu usuario>>/app-fastapi-fia

docker logs  fast-api-fia

```

> **NOTA**: A API vai ser atualizada automaticamente quando detectar mudanças no código.


### Executando os Testes

```
docker compose exec api pytest tests/
```