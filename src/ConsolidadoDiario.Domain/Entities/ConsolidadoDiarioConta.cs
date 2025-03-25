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