import pytest

@pytest.mark.order(1)
def test_get_aluno(api_client):
    response = api_client.get("/aluno/")
    assert response.status_code == 200
    assert response.json() == "Olá alunos"