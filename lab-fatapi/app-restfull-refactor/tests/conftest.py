import pytest
from fastapi.testclient import TestClient

from  app.main import app


@pytest.fixture(scope="function")
def api_client():
    return TestClient(app)



