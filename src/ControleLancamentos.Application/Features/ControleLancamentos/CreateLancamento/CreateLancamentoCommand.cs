using ControleLancamentos.Domain.Enums;
using MediatR;

namespace ControleLancamentos.Application.Features.ControleLancamentos.CreateLancamento
{
    public class CreateLancamentoCommand : IRequest<Guid>
    {
        public string Agencia { get; set; } = string.Empty;
        public string NumeroConta { get; set; } = string.Empty;        
        public decimal Valor { get; set; }        
        public TipoTransacao Tipo { get; set; }
        public string? Descricao { get; set; }        
    }
}