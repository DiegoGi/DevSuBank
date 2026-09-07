using DevSu.Bank.Customers.Application.Extensions;
using DevSu.Bank.Customers.Infrastructure.Extensions;
using DevSu.Bank.Customers.Presentation.Api.Extensions;
using DevSu.Bank.Customers.Presentation.Api.Middleware;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

string[] SupportedCultures = ["es", "en"];

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.SetMinimumLevel(Debugger.IsAttached);

// Add services to the container.
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApplicationServices(configuration);
builder.Services.AddPresentationApiServices();

var app = builder.Build();
app.MapHealthChecks("/status");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "DevSu.Bank.Customers.Presentation.Api v1");
    });
}

app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture(SupportedCultures[0])
    .AddSupportedCultures(SupportedCultures)
    .AddSupportedUICultures(SupportedCultures));

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

namespace DevSu.Bank.Customers.Presentation.Api
{
    public partial class Program
    {
        protected Program()
        {
        }
    }
}

