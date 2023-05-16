using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using gitPipeline.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<AdminManagement>(); // Register the AdminManagement service

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Add an endpoint for retrieving the maintenance schedule
app.MapGet("/maintenance", async (HttpContext context) =>
{
    var adminManagement = context.RequestServices.GetRequiredService<AdminManagement>();
    var maintenanceSchedule = await adminManagement.GetMaintenanceSchedule();

    var json = JsonSerializer.Serialize(maintenanceSchedule);
    await context.Response.WriteAsync(json);
});

app.Run();
