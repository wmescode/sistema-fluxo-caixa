
using ControleLancamentos.Domain.Entities.ContasBancarias;
using ControleLancamentos.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ControleLancamentos.ORM.Repositories
{
    public class ContaBancariaRepository : IContaBancariaRepository
    {
        private readonly DefaultContext _context;
        public ContaBancariaRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task AtualizarContaBancariaAsync(ContaBancaria contaBancaria, CancellationToken cancellationToken = default)
        {
            _context.Lancamentos.AddRange(contaBancaria.Lancamentos);
            _context.ContasBancarias.Update(contaBancaria);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ContaBancaria?> GetContaBancariaAsync(string numero, string agencia, CancellationToken cancellationToken = default)
        {
            return await _context.ContasBancarias
                .FirstOrDefaultAsync(c => c.Numero == numero && c.Agencia == agencia, cancellationToken);
        }

        public async Task<ContaBancaria?> GetContaBancariaWithLancamentoAsync(Guid lancamentoId, CancellationToken cancellationToken = default)
        {
            return await _context.ContasBancarias
                .AsNoTracking()
                .Where(c => c.Lancamentos.Any(l => l.Id == lancamentoId))
                .Include(c => c.Lancamentos
                    .Where(l => l.Id == lancamentoId)) 
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
