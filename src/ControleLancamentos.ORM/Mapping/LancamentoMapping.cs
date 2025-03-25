using ControleLancamentos.Domain.Entities.ContasBancarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleLancamentos.ORM.Mapping
{
    public class LancamentoMapping : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {            
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Data)
                .IsRequired();
            builder.Property(e => e.Valor)
                .IsRequired();
            builder.Property(e => e.Tipo)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(e => e.ContaId)
                .IsRequired();
            builder.Property(e => e.Descricao)
                .IsRequired(false);
            builder.Property(e => e.Estornado)
                .IsRequired();            
            builder.HasOne(e => e.Conta)
                .WithMany(e => e.Lancamentos)
                .HasForeignKey(e => e.ContaId);
        }
    }
}