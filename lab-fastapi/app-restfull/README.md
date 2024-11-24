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

## Padrões Restfull

![Estrutura](../../content/fastapi-02.png)


Vamos criar os arquivos `setup.py` e `docker-compose.yaml`

 ### Terminal do Linux
```bash
touch setup.py
touch docker-compose.yaml
```

 ### Terminal do Powershell
```powershell

# Cria um objeto de codificação UTF-8 sem BOM
$utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($false)

[System.IO.File]::WriteAllText("setup.py", "", $utf8NoBomEncoding)
[System.IO.File]::WriteAllText("docker-compose.yaml", "", $utf8NoBomEncoding)

```

### Editando o arquivo `setup.py` 


```python

import io
import os
from setuptools import find_packages, setup



def read(*paths, **kwargs):
    content = ""
    with io.open(
        os.path.join(os.path.dirname(__file__), *paths),
        encoding=kwargs.get("encoding", "utf8"),
    ) as open_file:
        content = open_file.read().strip()
    return content


def read_requirements(path):
    return [
        line.strip()
        for line in read(path).split("\n")
        if not line.startswith(('"', "#", "-", "git+"))
    ]

setup(
    name="FastFIA",
    version="0.1.0",
    description="Minha API",
    url="fia.io",
    python_requires=">=3.8",   
    author="Seu nome",
    packages=find_packages(exclude=["tests"]),
    include_package_data=True,
    install_requires=read_requirements("requirements.txt")     
)

```

### Editando o arquivo `requirements.txt` e adicionando mais bibliotecas


```plain
fastapi[standard]==0.115.4
pydantic
```


### Alterando  o arquivo `app/main.py`

```python
from pathlib import Path
from typing import Annotated, Any, List, Optional
from fastapi import FastAPI,HTTPException, status
from pydantic import BaseModel, EmailStr

app = FastAPI(
    title="Fast Api Fia",
    version="0.1.0",
    description="Minha api",
)

ALUNOS = [
    {"id": 1, "nome": "Pedro", "idade": 10, "email": "pedro@gmail.com", "senha": "1234"},
    {"id": 2, "nome": "Paulo", "idade": 20, "email": "paulo@gmail.com", "senha": "1234"},
    {"id": 3, "nome": "Gabriel", "idade": 35, "email": "gabriel@gmail.com", "senha": "1234"},
    {"id": 4, "nome": "Maria", "idade": 18, "email":"maria@gmail.com", "senha": "1234"}
]

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




@app.get("/")
async def home():
    return "Olá alunos"


@app.get("/alunos/", response_model=List[AlunoResponse], status_code=status.HTTP_200_OK)
async def Alunos()-> Any:
    return ALUNOS


@app.get("/alunos/{idAluno}", response_model=AlunoResponse, status_code=status.HTTP_200_OK)
async def Consultar_Aluno(idAluno: Annotated[int, Path(title="O ID do aluno para a consulta", ge=1)]) -> Any:
      for item in ALUNOS:
        if item["id"] == idAluno:
            return item
      raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")
      

@app.post("/alunos", response_model=AlunoResponse, status_code=status.HTTP_201_CREATED)
# async def Inserir_Aluno(aluno: str):
async def Inserir_Aluno(aluno: AlunoRequest)-> Any:
  for _aluno in ALUNOS:
        if _aluno["id"] == aluno.id:
            raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST, detail="Já existe um aluno com esse ID")
        
  ALUNOS.append(aluno.dict())
 
  return aluno
  

@app.put("/alunos/{idAluno}", response_model=AlunoResponse,  status_code=status.HTTP_200_OK)
async def Atualizar_Aluno(idAluno:int, aluno: AlunoRequest) -> Any:
 for _aluno in ALUNOS:
        if _aluno["id"] == aluno.id:
            _aluno["nome"] = aluno.nome
            _aluno["idade"] = aluno.idade
            _aluno["email"] = aluno.email
            _aluno["senha"] = aluno.senha
            
            return _aluno
 raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")


@app.delete("/alunos/{idAluno}", response_model=AlunoResponse, status_code=status.HTTP_200_OK)
async def Deletar_Aluno(idAluno: int) -> Any:
    for index, alnno in enumerate(ALUNOS):
        if alnno["id"] == idAluno:
            return ALUNOS.pop(index)
    raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Aluno não encontrado")


```

### Vamos editar o arquivo `Dockerfile`
```docker
FROM python:3.10
 
# Create the home directory
ENV APP_HOME=/home/app/api
RUN mkdir -p $APP_HOME
WORKDIR $APP_HOME

# install
COPY . $APP_HOME
RUN pip install --no-cache-dir --upgrade -r requirements.txt
RUN pip install -e .

 
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

Atualizando a imagem docker pelo arquivo DockerCompose

### Terminal Linux
```bash

docker container rm  fast-api-fia -f

docker compose down && docker-compose build --no-cache && docker-compose up -d

docker image ls

docker logs  fast-api-fia

```

### Terminal PowerShell
```powershell

docker container rm  fast-api-fia -f

docker compose down 
docker-compose build --no-cache 
docker-compose up -d

docker image ls

docker logs  fast-api-fia

```

> [!IMPORTANT]
> A API vai ser atualizada automaticamente quando detectar mudanças no arquivo



Acesse os endereços:

* http://localhost:8000/docs
* http://localhost:8000/openapi.json
* http://localhost:8000/redoc

---

3. [Vamos melhorar um pouco](../app-restfull-refactor/README.md)
4. [FastAPI e Esteira GitAction](../app-gitaction/README.md)
5. [FastAPI e Postgresql](../app-crud-db/README.md)
