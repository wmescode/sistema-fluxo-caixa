using ControleLancamentos.IoC;
using ControleLancamento.Api.ControleLancamentos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplications();

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
