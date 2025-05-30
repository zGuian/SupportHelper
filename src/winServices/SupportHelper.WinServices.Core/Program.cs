using SupportHelper.WinServices.Core;
using SupportHelper.WinServices.Core.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MachineWorker>();
builder.Services.AddDependencyInjection();

var host = builder.Build();
host.Run();
