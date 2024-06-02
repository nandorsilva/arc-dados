using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using worker_demo_kafka;

var Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .AddCommandLine(args)
               .Build();


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {

        Action<ResourceBuilder> appResourceBuilder =
                    resource => resource
                        .AddTelemetrySdk()
                        .AddService(Configuration.GetValue<string>("Otlp:ServiceName"));

        services.AddOpenTelemetry()
           .ConfigureResource(appResourceBuilder)
           .WithTracing(builder => builder
               .AddSource("Api.Otel.Debezium")
               .AddOtlpExporter(options => options.Endpoint = new Uri(Configuration.GetValue<string>("Otlp:Endpoint")))
           );


        services.AddHostedService<Worker>();
    }).UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .WriteTo.OpenTelemetry(otlpOptions =>
                {

                    otlpOptions.Endpoint = $"{Configuration.GetValue<string>("Otlp:Endpoint")}/v1/logs";

                    otlpOptions.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
                    otlpOptions.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = Configuration.GetValue<string>("Otlp:ServiceName")
                    };

                }))
    .Build();

await host.RunAsync();
