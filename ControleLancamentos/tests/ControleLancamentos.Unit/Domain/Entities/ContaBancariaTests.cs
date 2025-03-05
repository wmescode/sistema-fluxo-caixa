using FluentAssertions;
using ControleLancamentos.Domain.Enums;
using Xunit;
using ControleLancamentos.Unit.TestData;

namespace ControleLancamentos.Unit.Domain.Entities
{
    public class ContaBancariaTests
    {
        [Fact]
        public void GerarLancamento_DeveAdicionarLancamentoEAtualizarSaldo()
        {
            // Arrange
            var conta = TestDataGenerator.ContaBancariaFaker.Generate();
            decimal saldoInicial = conta.Saldo;

            // Act
            var lancamento = conta.GerarLancamento(100, TipoTransacao.Credito, "Teste");

            // Assert
            conta.Lancamentos.Should().Contain(lancamento);
            conta.Saldo.Should().Be(saldoInicial + 100);
        }

        [Fact]
        public void EstornarLancamento_DeveReverterSaldo()
        {
            // Arrange
            var conta = TestDataGenerator.ContaBancariaFaker.Generate();

            var lancamentoInicial = conta.GerarLancamento(200, TipoTransacao.Credito, "Venda Teste");
            var lancamentoEstornado = conta.GerarLancamento(200, TipoTransacao.Credito, "Compra Teste");

            // Act
            conta.EstornarLancamento(lancamentoEstornado.Id);

            // Assert
            conta.Saldo.Should().Be(conta.Saldo);
            lancamentoEstornado.Estornado.Should().BeTrue();
        }
    }
}
