using SupportHelper.Infrastructure.CrossCutting.IoC;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.IoC(builder.Configuration);
builder.Services.AddSignalR();
//builder.Services.AddHostedService<RabbitMQListenWorker>();
builder.Services.AddConfigurationApiVersioning();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapHub<ControlHub>("/SupportHelperConnectionSignalR");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
