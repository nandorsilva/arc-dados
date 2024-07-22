import pytest
from fastapi.testclient import TestClient

from  app.main import app


@pytest.fixture(scope="module")
def api_client():
    return TestClient(app)



