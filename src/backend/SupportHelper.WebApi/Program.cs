using SupportHelper.Application;
using SupportHelper.Infrastructure;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.WebApi.Configurations;
using SupportHelper.WebApi.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddApplicationContext(builder.Configuration);
builder.Services.AddInfrastructureContext(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddHostedService<RabbitMQListenWorker>();
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
