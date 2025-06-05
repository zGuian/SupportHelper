using SupportHelper.WinServices.Core;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDependencyInjection();

var host = builder.Build();
host.Run();
