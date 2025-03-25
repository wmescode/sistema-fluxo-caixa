using ControleLancamentos.IoC;
using ControleLancamento.Api.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplications();
LoggingConfigDI.AddLogConfig(builder.Configuration);
builder.Services.AddObservability();

builder.Host.UseSerilog();

var app = builder.Build();

app.AddControleLancamentosEndpoints();

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
