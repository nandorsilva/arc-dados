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

