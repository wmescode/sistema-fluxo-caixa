using Microsoft.EntityFrameworkCore;
using ConsolidadoDiario.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsolidadoDiario.ORM.Mapping
{
    public class ConsolidadoDiarioMapping : IEntityTypeConfiguration<ConsolidadoDiarioConta>
    {
        public void Configure(EntityTypeBuilder<ConsolidadoDiarioConta> builder)
        {            
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataConsolidacao)
                .IsRequired();
            builder.Property(x => x.TotalCreditos)
                .IsRequired();
            builder.Property(x => x.TotalDebitos)
                .IsRequired();
            builder.Property(x => x.SaldoConsolidado)
                .IsRequired();
            builder.Property(x => x.DataUltimaAtualizacao)
                .IsRequired();
        }
    }
}
