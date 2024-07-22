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

