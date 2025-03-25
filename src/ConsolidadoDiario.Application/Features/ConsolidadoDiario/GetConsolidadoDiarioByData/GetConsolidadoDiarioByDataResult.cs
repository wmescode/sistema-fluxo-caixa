
namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.GetConsolidadoDiarioByData
{
    public class GetConsolidadoDiarioByDataResult
    {
        public string NumeroConta { get; set; }
        public string NumeroAgencia { get; set; }
        public DateTime DataConsolidacao { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal SaldoConsolidado { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}
