using SupportHelper.WinServices.Core;
using SupportHelper.WinServices.Core.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDependencyInjection();
builder.Services.AddHostedService<MachineInfoWorker>();

var host = builder.Build();
host.Run();
