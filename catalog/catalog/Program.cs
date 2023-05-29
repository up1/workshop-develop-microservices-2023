using OpenTelemetry.Trace;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using System.Reflection.PortableExecutable;
using OpenTelemetry.Exporter;
using catalog;
using catalog.Services;
using AutoMapper;
using System.Text.Json.Serialization;
using Polly.Retry;
using Polly;

var appBuilder = WebApplication.CreateBuilder(args);
// Add services to the container.

appBuilder.Services.AddDbContext<DataContext>();
appBuilder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
appBuilder.Services.AddScoped<IProductService, ProductService>();
appBuilder.Services.AddHealthChecks();

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
            .AddSqlClientInstrumentation(options => options.SetDbStatementForText = true)
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

// Add circuit breaker
appBuilder.Services.AddHttpClient("Stock-Service", client =>
{
    client.BaseAddress = new Uri("http://stock:3000");
});

appBuilder.Services.AddHttpClient("Pricing-Service", client =>
{
    client.BaseAddress = new Uri("http://pricing:3000");
})
.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
{
    TimeSpan.FromSeconds(1),
    TimeSpan.FromSeconds(5),
    TimeSpan.FromSeconds(10)
}));

appBuilder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
appBuilder.Services.AddEndpointsApiExplorer();
appBuilder.Services.AddSwaggerGen();

var app = appBuilder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();

