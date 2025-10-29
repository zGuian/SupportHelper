using SupportHelper.API.Infra.CrossCutting.Bootstrapper;
using SupportHelper.API.Infra.SignalR.Hubs;
using SupportHelper.API.WebApi.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHostedService<QueueProcessWorker>();
builder.Services.AddHostedService<ConnectionsWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(pattern: "/swagger/v1/swagger.json");
    app.UseSwaggerUI(opts =>
    {
        opts.CacheLifetime = TimeSpan.FromSeconds(5);
        opts.DefaultModelsExpandDepth(-1);
        opts.DocumentTitle = "SupportHelper";
    });
}

app.UseAuthorization();

app.MapControllers();
app.MapHub<ControlHub>("/hubs/SupportHelperConnectionSignalR");
app.Run();
