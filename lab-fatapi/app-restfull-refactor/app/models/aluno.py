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
