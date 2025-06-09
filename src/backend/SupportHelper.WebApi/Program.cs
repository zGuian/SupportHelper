using SupportHelper.Application;
using SupportHelper.Infrastructure;
using SupportHelper.WebApi.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationContext(builder.Configuration);
builder.Services.AddInfrastructureContext(builder.Configuration);
builder.Services.AddHostedService<RabbitMQListenWorker>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
app.Run();
