using ControleLancamentos.Domain.Entities.Common;
using ControleLancamentos.Domain.Enums;

namespace ControleLancamentos.Domain.Entities.ContasBancarias
{
    public class Lancamento : BaseEntity
    {
        public Guid ContaId { get; set; }
        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime Data { get; set; }
        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; set; }
        /// <summary>
        /// Tipo do lançamento: débito ou crédito.
        /// </summary>
        public TipoTransacao Tipo { get; set; }        
        /// <summary>
        /// Descrição do lançamento
        /// </summary>
        public string? Descricao { get; set; }
        public bool Estornado { get; set; } = false;
        public ContaBancaria Conta { get; private set; }

        public Lancamento(decimal valor, TipoTransacao tipoTransacao, string? descricao, ContaBancaria conta)
        {            
            Id = Guid.NewGuid();
            ContaId = conta.Id;
            Data = DateTime.UtcNow;
            Valor = valor;
            Tipo = tipoTransacao;
            Descricao = descricao;
            Conta = conta;
        }
        public Lancamento(){}

        public void Estornar()
        {
            Estornado = true;
        }
    }
}
