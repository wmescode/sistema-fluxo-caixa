using ConsolidadoDiario.Api.Common;
using ConsolidadoDiario.Application.Features.ConsolidadoDiario.GetConsolidadoDiarioByData;
using MediatR;

namespace ConsolidadoDiario.Api.Endpoints
{
    public static class EndpointsConsolidadoDiario
    {
        public static void AddConsolidadoDiarioEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/consolidado-diario", async (
                [AsParameters] GetConsolidadoDiarioByDataQuery request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);
                return Results.Ok(new ApiResponseWithData<GetConsolidadoDiarioByDataResult>
                {
                    Success = true,
                    Message = "Consolidação obtida com sucesso",
                    Data = result
                });
            });

        }
    }
}
