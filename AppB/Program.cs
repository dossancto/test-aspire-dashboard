using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("Service B"))
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
        ;

    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
        // .AddEntityFrameworkCoreInstrdumentation()
        ;
    });

var exporterUri = new Uri("http://localhost:18889");
builder.Services.AddLogging(options =>
    options.AddOpenTelemetry(openTelemetry => openTelemetry.AddOtlpExporter(o => o.Endpoint = exporterUri)));

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder.AddOtlpExporter(o => o.Endpoint = exporterUri))
    .WithMetrics(metricProviderBuilder => metricProviderBuilder.AddOtlpExporter(o => o.Endpoint = exporterUri));

// builder.Services.AddHostedService<TimerJob>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("hello", () =>
{
    app.Logger.LogInformation("Request from service B");

    return new
    {
        Msg = "World"
    };
});

app.UseHttpsRedirection();

app.Run();
