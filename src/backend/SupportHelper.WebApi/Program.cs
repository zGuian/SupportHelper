using SupportHelper.Infrastructure.CrossCutting.IoC;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.WebApi.Configurations;
using SupportHelper.WebApi.Filters;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.IoC(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddConfigurationApiVersioning();
builder.Services.AddControllers()
    
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

var app = builder.Build();
app.MapHub<ControlHub>($"/{builder.Configuration["SignalR:OriginPath"]}");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
