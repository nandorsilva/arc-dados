using dev_kafka_demo;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<KafkaClientHandle>();
builder.Services.AddSingleton<KafkaDependentProducer<string, string>>();



Action<ResourceBuilder> configureResource =
   resourceBuilder =>
   {
       resourceBuilder.AddService(Configuration.GetValue<string>("Otlp:ServiceName"));
       resourceBuilder.AddTelemetrySdk();
   };

builder.Services.AddOpenTelemetry()
    .ConfigureResource(configureResource)
    .WithTracing(builder => builder
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddSource("Api.Otel")
    .AddOtlpExporter(options => options.Endpoint = new Uri(Configuration.GetValue<string>("Otlp:Endpoint")))
    )
     .WithMetrics(builder => builder
      .AddRuntimeInstrumentation()
      .AddAspNetCoreInstrumentation()
      .AddOtlpExporter(options => options.Endpoint = new Uri(Configuration.GetValue<string>("Otlp:Endpoint"))));




builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .WriteTo.OpenTelemetry(otlpOptions =>
    {

        otlpOptions.Endpoint = $"{Configuration.GetValue<string>("Otlp:Endpoint")}/v1/logs";

        otlpOptions.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
        otlpOptions.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = Configuration.GetValue<string>("Otlp:ServiceName")
        };

    }));


var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
