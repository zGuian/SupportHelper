using SupportHelper.RabbitMQ.Implementation;
using SupportHelper.RabbitMQ.Interfaces;
using SupportHelper.WinServices.Core;
using SupportHelper.WinServices.Core.Interfaces;
using SupportHelper.WinServices.Core.Services;
using SupportHelper.WinServices.Core.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<MachineWorker>();

builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddTransient<IMachineService, MachineService>();

var host = builder.Build();
host.Run();
