using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Infrastructure;
using SupportHelper.RabbitMQ.Interfaces;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddApplicationContext(builder.Configuration);
builder.Services.AddInfrastructureContext(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();
var client = app.Services.GetRequiredService<IRabbitMQRequestReply<MachineInformationRequest, 
    MachineInformationResponse>>();
await client.StartConsumerAsync();
app.UseAuthorization();
app.MapControllers();
app.Run();
