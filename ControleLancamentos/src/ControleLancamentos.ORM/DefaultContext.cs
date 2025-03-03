using ControleLancamentos.Domain.Entities.ContasBancarias;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ControleLancamentos.ORM
{
    public class DefaultContext : DbContext
    {
        public DbSet<ContaBancaria> ContasBancarias { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }

        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
        {
        }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // Verifica se já não foi configurado por injeção de dependência.
            {
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=DeveloperEvaluation;User Id=developer;Password=ev@luAt10n;TrustServerCertificate=True",
                    b => b.MigrationsAssembly(typeof(DefaultContext).Assembly.FullName)); // Configura o assembly de migrations
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }   
}
