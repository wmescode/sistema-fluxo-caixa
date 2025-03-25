using AutoMapper;
using ConsolidadoDiario.Domain.Repositories;
using ConsolidadoDiario.ORM;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.GetConsolidadoDiarioByData
{
    public class GetConsolidadoDiarioByDataHandler : IRequestHandler<GetConsolidadoDiarioByDataQuery, GetConsolidadoDiarioByDataResult>
    {
        private readonly IValidator<GetConsolidadoDiarioByDataQuery> _validator;
        private readonly IConsolidadoDiarioRepository _consolidadoDiarioRepository;
        private readonly IMapper _mapper;        
        private readonly DefaultContext _dbContext;        

        public GetConsolidadoDiarioByDataHandler(IValidator<GetConsolidadoDiarioByDataQuery> validator, 
                                                 IConsolidadoDiarioRepository consolidadoDiarioRepository,
                                                 IMapper mapper,                                                 
                                                 DefaultContext dbContext)
        {
            _validator = validator;
            _consolidadoDiarioRepository = consolidadoDiarioRepository;
            _mapper = mapper;            
            _dbContext = dbContext;            
        }

        public async Task<GetConsolidadoDiarioByDataResult> Handle(GetConsolidadoDiarioByDataQuery request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }           
            
            var result = await _dbContext.ConsolidadoDiarioContas
                .AsNoTracking()
                .Where(c => c.NumeroConta == request.NumeroContaBancaria &&
                            c.NumeroAgencia == request.AgenciaContaBancaria &&
                            c.DataConsolidacao.Date == request.Data.Date)
                .Select(c => new GetConsolidadoDiarioByDataResult
                {
                    NumeroConta = c.NumeroConta,
                    NumeroAgencia = c.NumeroAgencia,
                    DataConsolidacao = c.DataConsolidacao,
                    TotalCreditos = c.TotalCreditos,
                    TotalDebitos = c.TotalDebitos,
                    SaldoConsolidado = c.SaldoConsolidado,
                    DataUltimaAtualizacao = c.DataUltimaAtualizacao
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {                
                throw new KeyNotFoundException($"Consolidação diária não encontrada para a conta {request.NumeroContaBancaria} e agência {request.AgenciaContaBancaria} na data {request.Data.Date}");                
            }            

            return result;
        }
    }
}
