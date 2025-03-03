using ControleLancamento.Api.Common;
using ControleLancamentos.Application.Features.ControleLancamentos.CreateLancamento;
using MediatR;

namespace ControleLancamento.Api.ControleLancamentos
{
    public static class ModuloControleLancamentos
    {
        public static void AddControleLancamentosEndpoints (this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/lancamento", async (
                CreateLancamentoCommand request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);

                return Results.Created("", new ApiResponseWithData<Guid>
                {
                    Success = true,
                    Message = "Lançamento criado com sucesso",
                    Data = result
                });
            });


        }
    }
}
