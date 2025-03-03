using ControleLancamentos.Domain.Entities.ContasBancarias;

namespace ControleLancamentos.Domain.Repositories
{
    public interface IContaBancariaRepository
    {
        Task<ContaBancaria?> GetContaBancariaWithLancamentoAsync(Guid lancamentoId, CancellationToken cancellationToken = default);

        Task AtualizarContaBancariaAsync(ContaBancaria contaBancaria, CancellationToken cancellationToken = default);
        Task<ContaBancaria?> GetContaBancariaAsync(string numero, string agencia, CancellationToken cancellationToken = default);
    }
}
