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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
