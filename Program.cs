using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using gitPipeline.Data;
using Microsoft.Extensions.DependencyInjection;

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
    // Here, you can write the logic to retrieve the maintenance schedule
    // You can use the AdminManagement service or any other method to retrieve the schedule

    // For example, assuming you have an instance of AdminManagement available, you can do:
    var adminManagement = context.RequestServices.GetRequiredService<AdminManagement>();
    var maintenanceSchedule = await adminManagement.GetMaintenanceSchedule();

    await context.Response.WriteAsync(maintenanceSchedule);
});

app.Run();
