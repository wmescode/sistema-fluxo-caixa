using AutoMapper;
using ConsolidadoDiario.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.GetConsolidadoDiarioByData
{
    public class GetConsolidadoDiarioByDataHandler : IRequestHandler<GetConsolidadoDiarioByDataQuery, GetConsolidadoDiarioByDataResult>
    {
        private readonly IValidator<GetConsolidadoDiarioByDataQuery> _validator;
        private readonly IConsolidadoDiarioRepository _consolidadoDiarioRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetConsolidadoDiarioByDataHandler(IValidator<GetConsolidadoDiarioByDataQuery> validator, 
                                                 IConsolidadoDiarioRepository consolidadoDiarioRepository,
                                                 IMapper mapper,
                                                 IDistributedCache cache)
        {
            _validator = validator;
            _consolidadoDiarioRepository = consolidadoDiarioRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<GetConsolidadoDiarioByDataResult> Handle(GetConsolidadoDiarioByDataQuery request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            // Chave única para o cache
            var cacheKey = $"ConsolidadoDiario_{request.NumeroContaBancaria}_{request.AgenciaContaBancaria}_{request.Data:yyyy-MM-dd}";

            // Tenta obter do cache
            var cachedResult = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (cachedResult != null)
            {                
                return JsonSerializer.Deserialize<GetConsolidadoDiarioByDataResult>(cachedResult)!;
            }

            var consolidadoDiarioConta = await _consolidadoDiarioRepository.GetConsolidadoDiarioContaAsync(request.NumeroContaBancaria, request.AgenciaContaBancaria, request.Data, cancellationToken);

            if(consolidadoDiarioConta == null)
            {
                throw new KeyNotFoundException ($"Consolidação diária não encontrada para a conta {request.NumeroContaBancaria} e agência {request.AgenciaContaBancaria} na data {request.Data.Date}");
            }

            var result = _mapper.Map<GetConsolidadoDiarioByDataResult>(consolidadoDiarioConta);

            // Armazena no cache com expiração de 1 hora
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            }, cancellationToken);

            return result;
        }
    }
}
