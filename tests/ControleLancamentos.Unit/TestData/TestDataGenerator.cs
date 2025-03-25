
using Bogus;
using ControleLancamentos.Domain.Entities.ContasBancarias;
using ControleLancamentos.Domain.Enums;

namespace ControleLancamentos.Unit.TestData
{
    public static class TestDataGenerator
    {
        public static Faker<ContaBancaria> ContaBancariaFaker => new Faker<ContaBancaria>()
            .CustomInstantiator(f => new ContaBancaria(
                f.Name.FullName(),
                f.Finance.Account(),
                f.Random.Int(1000, 9999).ToString(),
                f.PickRandom<TipoContaBancaria>(),
                f.Finance.Amount(100, 10000)
            ));

        public static Faker<Lancamento> LancamentoFaker(ContaBancaria conta) => new Faker<Lancamento>()
            .CustomInstantiator(f => new Lancamento(
                f.Finance.Amount(1, 1000),
                f.PickRandom<TipoTransacao>(),
                f.Lorem.Sentence(),
                conta
            ));
    }
}
