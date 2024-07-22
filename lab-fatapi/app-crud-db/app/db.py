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