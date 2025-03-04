using ConsolidadoDiario.Domain.Entities;
using ConsolidadoDiario.Domain.Repositories;
using MediatR;

namespace ConsolidadoDiario.Application.Features
{
    public class UpdateConsolidadoDiarioHandler : IRequestHandler<UpdateConsolidadoDiarioCommand>
    {
        private readonly IConsolidadoDiarioRepository _consolidadoDiarioRepository;
        public UpdateConsolidadoDiarioHandler(IConsolidadoDiarioRepository consolidadoDiarioRepository)
        {
            _consolidadoDiarioRepository = consolidadoDiarioRepository;
        }

        public async Task Handle(UpdateConsolidadoDiarioCommand request, CancellationToken cancellationToken)
        {
            var consolidadoDiario = await _consolidadoDiarioRepository.GetConsolidadoDiarioContaAsync(request.NumeroContaBancaria, request.AgenciaContaBancaria, request.Data, cancellationToken);

            if(consolidadoDiario == null)
            {                
                consolidadoDiario = new ConsolidadoDiarioConta(request.NumeroContaBancaria, request.AgenciaContaBancaria, request.Data);
                consolidadoDiario.AtualizaSaldoConsolidado(request.Tipo, request.Valor);
                await _consolidadoDiarioRepository.AddConsolidadoDiarioContaAsync(consolidadoDiario, cancellationToken);
            }
            else
            {
                consolidadoDiario.AtualizaSaldoConsolidado(request.Tipo, request.Valor);
                await _consolidadoDiarioRepository.UpdateConsolidadoDiarioContaAsync(consolidadoDiario, cancellationToken);
            }

            

            //verifica se já existe consolidado diario para a data e conta informada
            //atualizar o consolidado diario no banco de dados postgresql
            //atualizar consolidado diario no cache redis
            //avaliar atomocidade entre persistencia no redis e no postgresql            
        }
    }
}
