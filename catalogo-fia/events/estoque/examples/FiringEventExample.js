
[HttpPost(Name = "ProduzirMensagen")]
public ActionResult  Post(MensagemAluno mensagem)
{
    var config = new ProducerConfig
    {
        BootstrapServers = _bootstrapServers
    };


    string mensagemJSON = JsonSerializer.Serialize(mensagem);

    
    using (var producer = new ProducerBuilder<int, string>(config).Build())
    {
        var resultado = producer.ProduceAsync(TOPIC_ALUNO,
            new Message<int, string>
            {
                Key = mensagem.Id,
                Value = mensagemJSON
            }).GetAwaiter().GetResult();

        _logger.LogInformation($"Mensagem: {mensagemJSON} | Status: {resultado.Status.ToString()}");
    }

    return Ok();
}