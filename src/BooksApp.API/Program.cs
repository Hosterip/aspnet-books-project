using PostsApp.Application;
using PostsApp.Common.Extensions;
using PostsApp.Infrastructure;
using PostsApp.Middlewares;
using Toycloud.AspNetCore.Mvc.ModelBinding;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
var corsAllow = "CorsAllow";
builder.Services.AddCorsPolicy(corsAllow);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.InsertBodyOrDefaultBinding();
});
// Adding Dependency Injectable 
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

// Authentication || Authorization

builder.Services.AddAuth();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(corsAllow);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidateUserMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();

app.Run();