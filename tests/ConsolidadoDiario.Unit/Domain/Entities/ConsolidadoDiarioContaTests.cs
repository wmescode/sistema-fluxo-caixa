/*
 using ConsolidadoDiario.Domain.Common;
using ConsolidadoDiario.Domain.Enums;

namespace ConsolidadoDiario.Domain.Entities
{
    public class ConsolidadoDiarioConta : BaseEntity
    {
        public string NumeroConta { get; private set; }
        public string NumeroAgencia { get; private set; }
        public DateTime DataConsolidacao { get; private set; }
        public decimal TotalCreditos { get; private set; }
        public decimal TotalDebitos { get; private set; }
        public decimal SaldoConsolidado { get; private set; }
        public DateTime DataUltimaAtualizacao { get; private set; }
        
        public ConsolidadoDiarioConta(){}

        public ConsolidadoDiarioConta(string numeroConta, 
                                      string numeroAgencia, 
                                      DateTime dataConsolidacao)
        {
            Id = Guid.NewGuid();
            NumeroConta = numeroConta;
            NumeroAgencia = numeroAgencia;
            DataConsolidacao = dataConsolidacao;
        }

        public void AtualizaSaldoConsolidado(TipoTransacao tipoTransacao, decimal valor)
        {
            if (tipoTransacao == TipoTransacao.Credito)
            {
                TotalCreditos += valor;
            }
            else
            {
                TotalDebitos += valor;
            }
            
            SaldoConsolidado = TotalCreditos - TotalDebitos;
            DataUltimaAtualizacao = DateTime.UtcNow;
        }

    }
}
 
 */
using ConsolidadoDiario.Domain.Entities;
using ConsolidadoDiario.Domain.Enums;
using ConsolidadoDiario.Unit.TestData;
using FluentAssertions;

namespace ConsolidadoDiario.Unit.Domain.Entities
{
    public class ConsolidadoDiarioContaTests
    {
        [Fact]
        public void CriarConsolidadoDiarioConta_ValoresValidos_DeveCriarInstancia()
        {
            // Arrange
            var consolidadoDiarioConta = TestDataGenerator.GenerateConsolidadoDiarioConta().Generate();                        
            // Assert
            consolidadoDiarioConta.Should().NotBeNull();
            consolidadoDiarioConta.NumeroConta.Should().NotBeNullOrEmpty();
            consolidadoDiarioConta.NumeroAgencia.Should().NotBeNullOrEmpty();
            consolidadoDiarioConta.DataConsolidacao.Should().BeAfter(DateTime.MinValue);
        }
        //AtualizaSaldoConsolidado
        [Fact]
        public void AtualizaSaldoConsolidado_ValorCredito_DeveAtualizarTotalCreditos()
        {
            // Arrange
            var consolidadoDiarioConta = TestDataGenerator.GenerateConsolidadoDiarioConta().Generate();
            var saldoCreditoAnterior = consolidadoDiarioConta.TotalCreditos;
            var saldoConsolidadoAnterior = consolidadoDiarioConta.SaldoConsolidado;
            var valorCredito = 100m;
            // Act
            consolidadoDiarioConta.AtualizaSaldoConsolidado(TipoTransacao.Credito, valorCredito);
            // Assert
            consolidadoDiarioConta.TotalCreditos.Should().Be(valorCredito + saldoCreditoAnterior);
            consolidadoDiarioConta.SaldoConsolidado.Should().Be(valorCredito + saldoConsolidadoAnterior);
        }
    }
}
