using FluentValidation;
namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.GetConsolidadoDiarioByData
{
    public class GetConsolidadoDiarioByDataValidator : AbstractValidator<GetConsolidadoDiarioByDataQuery>
    {
        public GetConsolidadoDiarioByDataValidator()
        {
            RuleFor(x => x.Data)
                .NotEmpty().WithMessage("Data é obrigatória")
                .LessThan(DateTime.Now).WithMessage("Data não pode ser maior que a data atual");

            RuleFor(x => x.NumeroContaBancaria)
                .NotEmpty().WithMessage("Número da conta bancária é obrigatório");

            RuleFor(x => x.AgenciaContaBancaria)
                .NotEmpty().WithMessage("Agência da conta bancária é obrigatória");
        }
    }
}
