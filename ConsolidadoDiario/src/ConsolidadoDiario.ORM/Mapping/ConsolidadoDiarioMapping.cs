using Microsoft.EntityFrameworkCore;
using ConsolidadoDiario.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
/*
 	tabela ConsolidadoDiario
			data
			totalCreditos
			totalDebitos
			saldoConsolidado - Diferença entre créditos e débitos (Total de Créditos - Total de Débitos)
			data Última Atualização
 
 */
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
