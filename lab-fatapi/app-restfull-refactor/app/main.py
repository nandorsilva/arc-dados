from fastapi import FastAPI

from .routes import main_router

app = FastAPI(
    title="Fast Api Fia",
    version="0.1.0",
    description="Minha api",
)

app.include_router(main_router)
