using Bogus;
using ConsolidadoDiario.Domain.Entities;

namespace ConsolidadoDiario.Unit.TestData
{
    public static class TestDataGenerator
    {
        public static Faker<ConsolidadoDiarioConta> GenerateConsolidadoDiarioConta()
        {
            return new Faker<ConsolidadoDiarioConta>()
                .RuleFor(c => c.NumeroConta, f => f.Finance.Account())
                .RuleFor(c => c.NumeroAgencia, f => f.Finance.Account())
                .RuleFor(c => c.DataConsolidacao, f => f.Date.Past(1))
                .RuleFor(c => c.TotalCreditos, f => f.Finance.Amount(0, 10000))
                .RuleFor(c => c.TotalDebitos, f => f.Finance.Amount(0, 10000))
                .RuleFor(c => c.SaldoConsolidado, (f, c) => c.TotalCreditos - c.TotalDebitos)
                .RuleFor(c => c.DataUltimaAtualizacao, f => f.Date.Recent());
        }
    }
}
