using ControleLancamentos.Domain.Entities.Common;
using ControleLancamentos.Domain.Enums;

namespace ControleLancamentos.Domain.Entities.ContasBancarias
{
    public class ContaBancaria : BaseEntity
    {        
        public string Nome { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Agencia { get; set; } = string.Empty;        
        public TipoContaBancaria Tipo { get; set; } 
        public decimal Saldo { get; set; }
        public ICollection<Lancamento> Lancamentos { get; private set; } = new List<Lancamento>();

        public ContaBancaria(string nome, string numero, string agencia, TipoContaBancaria tipo, decimal saldo)
        {
            Nome = nome;
            Numero = numero;
            Agencia = agencia;
            Tipo = tipo;            
        }

        public ContaBancaria(){}

        /// <summary>
        /// Gera um lançamento na conta bancária
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tipoTransacao"></param>
        /// <param name="descricao"></param>
        public Lancamento GerarLancamento(decimal valor, TipoTransacao tipoTransacao, string? descricao)
        {
            var lancamento = new Lancamento(valor, tipoTransacao, descricao, this);

            Lancamentos.Add(lancamento);

            if (lancamento.Tipo == TipoTransacao.Credito)
            {
                Saldo += lancamento.Valor;
            }
            else
            {
                Saldo -= lancamento.Valor;
            }
            
            return lancamento;
        }

        /// <summary>
        /// Estorna um lançamento da conta bancária
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void EstornarLancamento(Guid id)
        {
            try
            {
                var lancamento = Lancamentos.Where(a => a.Id == id).First();
                lancamento.Estornar();

                if (lancamento.Tipo == TipoTransacao.Credito)                
                    Saldo -= lancamento.Valor;                
                else                
                    Saldo += lancamento.Valor;
            }
            catch
            {
                throw new Exception("Lançamento não encontrado");
            }
        }
    }
}
