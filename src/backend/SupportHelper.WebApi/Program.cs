using SupportHelper.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddApplicationContext(builder.Configuration);
builder.Services.AddInfrastructureContext(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
app.Run();
