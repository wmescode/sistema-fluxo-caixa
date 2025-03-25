using Autofac.Extensions.DependencyInjection;
using ConsolidadoDiario.Api.Endpoints;
using ConsolidadoDiario.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplications();
LoggingConfigDI.AddLogConfig(builder.Configuration);
builder.Services.AddObservability();

builder.Host.UseSerilog();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

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