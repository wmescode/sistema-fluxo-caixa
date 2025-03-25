using ControleLancamentos.Domain.Events.ControleLancamentos;
using ControleLancamentos.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace ControleLancamentos.Application.Features.ControleLancamentos.CreateLancamento
{
    public class CreateLancamentoHandler : IRequestHandler<CreateLancamentoCommand, Guid>
    {   
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IMediator _mediator;
        public CreateLancamentoHandler(IContaBancariaRepository contaBancariaRepository, 
                                       IMediator mediator)
        {
            _contaBancariaRepository = contaBancariaRepository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateLancamentoCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLancamentoValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {                
                throw new ValidationException(validationResult.Errors);
            }

            var contaBancaria = await _contaBancariaRepository.GetContaBancariaAsync(request.NumeroConta, request.Agencia);
            if(contaBancaria == null)
            {                
                throw new KeyNotFoundException($"Conta Bancarária com Agência {request.Agencia} e Conta {request.Agencia} não encontrada");
            }

            var lancamento = contaBancaria.GerarLancamento(request.Valor, request.Tipo, request.Descricao);

            await _contaBancariaRepository.AtualizarContaBancariaAsync(contaBancaria, cancellationToken);

            var lancamentoCreatedEvent = new LancamentoCreatedEvent(lancamento.Id, request.NumeroConta, request.Agencia, request.Tipo, request.Valor, lancamento.Data);
            
            await _mediator.Publish(lancamentoCreatedEvent, cancellationToken);

            return lancamento.Id;
        }
    }
}