using SupportHelper.RabbitMQ.Implementation;
using SupportHelper.RabbitMQ.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var rabbitConnection = await RabbitMQConnection.CreateConnectionToRabbitMQ(builder.Configuration);
builder.Services.AddSingleton<IRabbitMQConnection>(rabbitConnection);
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
