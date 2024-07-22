from pydantic import BaseSettings
import os

class Settings(BaseSettings):
    API_V1_STR: str = '/api/v1'
    DB_URL: str =  os.environ.get("DATABASE_URL")
    ##DB_URL: str = 'postgresql+asyncpg://postgres:postgres@db-postgres-fastapi:5432/dbfiafastapi'  


settings: Settings = Settings()
