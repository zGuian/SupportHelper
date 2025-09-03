using Microsoft.AspNetCore.Mvc.Razor;
using SupportHelper.FrontEnd.MVC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.IoC(builder.Configuration);

builder.Services.AddControllersWithViews()
    .AddRazorOptions(opts =>
    {
        opts.ViewLocationFormats.Add("/Views/PartialsViews/{0}" + RazorViewEngine.ViewExtension);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
