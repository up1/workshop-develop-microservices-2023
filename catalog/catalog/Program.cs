using OpenTelemetry.Trace;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using System.Reflection.PortableExecutable;
using OpenTelemetry.Exporter;

var appBuilder = WebApplication.CreateBuilder(args);
// Add services to the container.
Action<ResourceBuilder> configureResource = r => r.AddService(
    serviceName: "catalog-service",
    serviceVersion: "1.0",
    serviceInstanceId: "catalog");

appBuilder.Services.AddOpenTelemetry()
    .ConfigureResource(configureResource)
    .WithTracing(builder =>
    {
        builder
            .AddSource(".NET7 Core")
            .SetSampler(new AlwaysOnSampler())
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation();

        appBuilder.Services.Configure<AspNetCoreInstrumentationOptions>(appBuilder.Configuration.GetSection("AspNetCoreInstrumentation"));

        builder.AddJaegerExporter();
        builder.ConfigureServices(services =>
        {
            services.Configure<JaegerExporterOptions>(appBuilder.Configuration.GetSection("Jaeger"));
            services.AddHttpClient("JaegerExporter", configureClient: (client) => client.DefaultRequestHeaders.Add("X-MyCustomHeader", "value"));
        });

    });

appBuilder.Services.AddControllers();
appBuilder.Services.AddEndpointsApiExplorer();
appBuilder.Services.AddSwaggerGen();

var app = appBuilder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

