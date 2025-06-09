using SupportHelper.WinServices.Core;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDependencyInjection();

Console.WriteLine($"Routing Key is [worker.machine.{Environment.MachineName.ToLower()}]");

var host = builder.Build();
host.Run();
