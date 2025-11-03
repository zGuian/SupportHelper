using SupportHelper.Service.Application.Workers;
using SupportHelper.Service.CrossCutting.Bootstrapper;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddHostedService<SignalRWorker>();

var host = builder.Build();
host.Run();