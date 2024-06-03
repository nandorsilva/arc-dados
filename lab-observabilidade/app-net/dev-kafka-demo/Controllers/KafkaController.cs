using Confluent.Kafka;
using dev_kafka_demo.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

namespace dev_kafka_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KafkaController : ControllerBase
    {
        private readonly ILogger<KafkaController> _logger;
        private readonly string _topic;

        private readonly KafkaDependentProducer<string, string> producer;

        //O nome da Activity deve ser igual ao nome da fonte adicionada na inicialização da API Web AddOpenTelemetryTracing Builder
        private static readonly ActivitySource Activity = new("Api.Otel");
        private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;


        public KafkaController(ILogger<KafkaController> logger, IConfiguration configuration, KafkaDependentProducer<string, string> producer)
        {
            this._logger = logger;
            this.producer = producer;


            this._topic = configuration["Topic"];
        }

        [HttpPost(Name = "ProduzirMensagen")]
        public async Task<IActionResult> Post(MensagemProduto mensagem)
        {

            string mensagemJSON = JsonSerializer.Serialize(mensagem);

            //Transformação de dados
            var message = new Message<string, string>
            {
                Key = $"{{\"_id\":{mensagem.Id}}}",
                Value = mensagemJSON,
                Headers = new Headers()
            };

            using (var activity = Activity.StartActivity("producer.mensagem", ActivityKind.Producer))
            {

                await this.producer.ProduceAsync(this._topic, message);

                AddActivityToHeader(activity, message.Headers);

            }

          
            Console.WriteLine($"Send message Tipic: {this._topic} message: {mensagemJSON}");

            _logger.LogInformation($"Send message Tipic: {this._topic} message: {mensagemJSON}");


            return Ok();
        }

        private void AddActivityToHeader(Activity activity, Headers headers)

        {
            try
            {
                Propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), headers, InjectContextIntoHeader);
                activity?.SetTag("messaging.system", "kafka");
                activity?.SetTag("messaging.destination_kind", "topic");
                activity?.SetTag("messaging.kafka.topic", this._topic);
                activity?.SetTag("messaging.destination", "Processing");
            }
            catch (Exception ex)
            {
                var t = ex.Message;
            }
        }


        private void InjectContextIntoHeader(Headers headers, string key, string value)
        {
            try
            {
                headers ??= new Headers();
                headers.Add(key, Encoding.UTF8.GetBytes(value));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to inject trace context");
            }
        }
    }
}