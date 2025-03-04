using ConsolidadoDiario.Domain.Entities;

namespace ConsolidadoDiario.Domain.Repositories
{
    public interface IConsolidadoDiarioRepository
    {
        Task<ConsolidadoDiarioConta?> GetConsolidadoDiarioContaAsync(string numeroConta, string numeroAgencia, DateTime dataConsolidacao, CancellationToken cancellationToken = default);
        Task AddConsolidadoDiarioContaAsync(ConsolidadoDiarioConta consolidadoDiarioConta, CancellationToken cancellationToken = default);
        Task UpdateConsolidadoDiarioContaAsync(ConsolidadoDiarioConta consolidadoDiarioConta, CancellationToken cancellationToken = default);
    }
}
