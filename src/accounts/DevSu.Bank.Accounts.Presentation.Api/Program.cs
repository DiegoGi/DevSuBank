using DevSu.Bank.Accounts.Application.Extensions;
using DevSu.Bank.Accounts.Infrastructure.Extensions;
using DevSu.Bank.Accounts.Presentation.Api.Extensions;
using DevSu.Bank.Accounts.Presentation.Api.Middleware;
using System.Diagnostics;

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
        options.SwaggerEndpoint("/openapi/v1.json", "DevSu.Bank.Accounts.Presentation.Api v1");
    });
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

namespace DevSu.Bank.Accounts.Presentation.Api
{
    public partial class Program
    {
        protected Program()
        {
        }
    }
}

