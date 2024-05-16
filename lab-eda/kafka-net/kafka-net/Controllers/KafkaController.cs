using Confluent.Kafka;
using kafka_net.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace kafka_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly ILogger<KafkaController> _logger;
        private const string TOPIC_ALUNO = "aluno";
        private readonly string _bootstrapServers;

        public KafkaController(ILogger<KafkaController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _bootstrapServers= configuration["BootstrapServers"];
        }

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

    }
}
