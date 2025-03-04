using ConsolidadoDiario.Domain.Entities;
using ConsolidadoDiario.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConsolidadoDiario.ORM.Repositories
{
    public class ConsolidadoDiarioRepository : IConsolidadoDiarioRepository
    {
        private readonly DefaultContext _context;
        public ConsolidadoDiarioRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task AddConsolidadoDiarioContaAsync(ConsolidadoDiarioConta consolidadoDiarioConta, CancellationToken cancellationToken = default)
        {
            await _context.ConsolidadoDiarioContas.AddAsync(consolidadoDiarioConta, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ConsolidadoDiarioConta?> GetConsolidadoDiarioContaAsync(string numeroConta, string numeroAgencia, DateTime dataConsolidacao, CancellationToken cancellationToken = default)
        {
            return await _context.ConsolidadoDiarioContas
                .Where(c => c.NumeroConta == numeroConta && 
                            c.NumeroAgencia == numeroAgencia && 
                            c.DataConsolidacao.Date == dataConsolidacao.Date)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateConsolidadoDiarioContaAsync(ConsolidadoDiarioConta consolidadoDiarioConta, CancellationToken cancellationToken = default)
        {
            _context.ConsolidadoDiarioContas.Update(consolidadoDiarioConta);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
