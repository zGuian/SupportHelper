using SupportHelper.WinServices.Core;
using SupportHelper.WinServices.Core.Settings;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDependencyInjection();
builder.Services.Configure<SignalrSettings>(
    builder.Configuration.GetSection("SignalrSettings"));

Console.WriteLine(Environment.MachineName);

var host = builder.Build();
host.Run();
