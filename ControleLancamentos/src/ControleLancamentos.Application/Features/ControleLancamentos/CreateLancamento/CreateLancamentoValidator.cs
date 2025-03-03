using FluentValidation;

namespace ControleLancamentos.Application.Features.ControleLancamentos.CreateLancamento
{
    public class CreateLancamentoValidator : AbstractValidator<CreateLancamentoCommand>
    {
        public CreateLancamentoValidator()
        {
            RuleFor(x => x.Valor).GreaterThan(0).WithMessage("O valor do lançamento deve ser maior que zero.");
            RuleFor(x => x.Tipo).IsInEnum().WithMessage("Tipo de transação financeira inválida.");
            RuleFor(x => x.NumeroConta).NotEmpty().WithMessage("O número da conta bancária é obrigatório.");
            RuleFor(x => x.Agencia).NotEmpty().WithMessage("O número da agência bancária é obrigatório.");
        }
    }
}
