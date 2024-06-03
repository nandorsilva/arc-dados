using Confluent.Kafka;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;
using static Confluent.Kafka.ConfigPropertyNames;

namespace worker_demo_kafka
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumer<Ignore, string> kafkaConsumer;
        private readonly string topic;
        private readonly string hostEndPoint;

        private static readonly ActivitySource Activity = new("Api.Otel.Debezium");
        private static readonly TextMapPropagator Propagator = new TraceContextPropagator();

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;

            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            Console.WriteLine("Worker started at: {0}", DateTimeOffset.Now);


            //Logar o "Kafka:TopicConsummer 
            _logger.LogInformation("Kafka:TopicConsummer: {topic}", config.GetValue<string>("Kafka:TopicConsummer"));

            //logar o bootstrapServers
            _logger.LogInformation("Kafka:BootstrapServers: {bootstrapServers}", config.GetValue<string>("Kafka:ConsumerSettings:BootstrapServers"));


            var consumerConfig = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
            this.topic = config.GetValue<string>("Kafka:TopicConsummer");

            this.hostEndPoint=config.GetValue<string>("HostEndPoint");
            
            this.kafkaConsumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

            //var consumerConfig = new ConsumerConfig
            //{
            //    BootstrapServers = "localhost:9092",
            //    GroupId = "_consumer_group_otel_id_sqldebezium",
            //    AutoOffsetReset = AutoOffsetReset.Latest // Choose the appropriate offset reset strategy
            //                                             // Add other Kafka consumer configuration as needed
            //};



         
            Console.WriteLine("Worker Finish at: {0}", DateTimeOffset.Now);

            _logger.LogInformation("Worker Finish at: {time}", DateTimeOffset.Now);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {          

            await Task.Run(() => StartConsumerKafka(stoppingToken), stoppingToken);
        }

        private void StartConsumerKafka(CancellationToken stoppingToken)
        {
            this.kafkaConsumer.Subscribe(this.topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = this.kafkaConsumer.Consume(stoppingToken);

                    if (consumeResult != null)
                    {
                        if (consumeResult.IsPartitionEOF)
                        {
                            Console.WriteLine($"End of partition: {consumeResult.TopicPartitionOffset}");
                            continue;
                        }


                        if (consumeResult.Message.Value is null) continue;

                        var message = consumeResult.Message.Value;
                        var headers = consumeResult.Message.Headers;

                        // Extract parent context from Kafka headers
                        var parentContext = Propagator.Extract(default, headers, ExtractTraceContextFromHeaders);
                        Baggage.Current = parentContext.Baggage;

                        _logger.LogInformation($"Received message at {consumeResult.TopicPartitionOffset}: {message}");

                        Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: {message}");
                                              

                        using (var activity = Activity.StartActivity("Consumer.mensagem", ActivityKind.Consumer, parentContext.ActivityContext))
                        {                          
                            EnviarMensagemApi(message, headers, activity);
                        }

                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
             
            }
        }

        private IEnumerable<string> ExtractTraceContextFromHeaders(Headers headers, string key)
        {
            try
            {
                if (headers.TryGetLastBytes(key, out var value))
                {
                    return new[] { Encoding.UTF8.GetString(value) };
                }
            }
            catch (Exception ex)
            {
                // Handle extraction exception
            }

            return Enumerable.Empty<string>();
        }

        private void EnviarMensagemApi(string mensagem, Headers headers, Activity? activity)
        {

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("traceparent", "00-" + activity.TraceId.ToHexString() + "-" + activity.SpanId.ToHexString() + "-01");
            client.BaseAddress = new Uri(this.hostEndPoint);

            
            var content = new StringContent(mensagem, Encoding.UTF8, "application/json");
            var response = client.PostAsync("/api/Kafka", content).Result;

        }
    }
}