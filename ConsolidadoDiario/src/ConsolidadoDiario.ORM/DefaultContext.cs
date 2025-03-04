using ConsolidadoDiario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ConsolidadoDiario.ORM
{
    public class DefaultContext : DbContext
    {
        public DbSet<ConsolidadoDiarioConta> ConsolidadoDiarioContas { get; set; }

        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
