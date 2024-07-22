from fastapi import APIRouter

from .aluno import router as aluno_router

main_router = APIRouter()

main_router.include_router(aluno_router, prefix="/aluno", tags=["aluno"])

