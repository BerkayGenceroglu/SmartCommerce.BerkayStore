using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentWorker.Consumers;
using PaymentWorker.Context;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = Host.CreateApplicationBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
        new Uri(builder.Configuration["Elasticsearch:Uri"]!))
    {
        IndexFormat = "paymentworker-logs-{0:yyyy.MM}",
        AutoRegisterTemplate = true
    })
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// DbContext
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MassTransit + RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", 5674, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]!);
            h.Password(builder.Configuration["RabbitMq:Password"]!);
        });

        cfg.ReceiveEndpoint("payment-order-created", e => // ← bunu ekle
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();