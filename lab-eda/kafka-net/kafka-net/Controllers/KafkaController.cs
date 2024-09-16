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
        private const string TOPIC_ALUNO = "agendamento-criado-test";
        private readonly string _bootstrapServers;

        public KafkaController(ILogger<KafkaController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _bootstrapServers = configuration["BootstrapServers"];
        }

        [HttpPost(Name = "ProduzirMensagen")]
        public  ActionResult Post(MensagemAluno mensagem)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                //Acks = Acks.Leader,
                //MessageTimeoutMs = 10000, // 60 segundos
                //LingerMs=5,
                //BatchSize = 16384     ,
                //RequestTimeoutMs= 30000,
                //RetryBackoffMs= 1000,
                //ClientId="cli"

            };


            string mensagemJSON = JsonSerializer.Serialize(mensagem);


            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var _mensagem = new Message<string, string>()
                {
                    Key = mensagem.Id.ToString(),
                    Value = mensagemJSON
                };


                //var resultado = producer.ProduceAsync(TOPIC_ALUNO,
                //    new Message<int, string>
                //    {
                //        //Key = mensagem.Id,
                //        Value = mensagemJSON
                //    }).GetAwaiter().GetResult();

                try
                {

                    var resultador = producer.ProduceAsync(TOPIC_ALUNO, _mensagem).GetAwaiter().GetResult() ;//.ConfigureAwait(false);

                    producer.Produce(TOPIC_ALUNO, _mensagem);
                }
                catch (ProduceException<string, string> e)
                {

                    var sss = e;
                }


                //_logger.LogInformation($"Mensagem: {mensagemJSON} | Status: {resultado.Status.ToString()}");
            }

            return Ok();
        }

    }
}
