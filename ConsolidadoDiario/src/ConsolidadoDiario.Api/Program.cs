using ConsolidadoDiario.Api.Endpoints;
using ConsolidadoDiario.IoC;
using FluentValidation;
using Serilog;
using Serilog.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplications();
DISerilogExtensions.AddLogConfig(builder.Configuration);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


builder.Services.AddAllElasticApm();
builder.Services.AddLogging(lb => lb.AddSerilog());
builder.Host.UseSerilog();
builder.Services.AddElasticApm();

builder.Services.AddElasticApmForAspNetCore(
    new Elastic.Apm.DiagnosticSource.IDiagnosticsSubscriber[]
    {
        new Elastic.Apm.AspNetCore.DiagnosticListener.AspNetCoreDiagnosticSubscriber(),
        new Elastic.Apm.EntityFrameworkCore.EfCoreDiagnosticsSubscriber(),
    }
);

var app = builder.Build();

app.AddConsolidadoDiarioEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();