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