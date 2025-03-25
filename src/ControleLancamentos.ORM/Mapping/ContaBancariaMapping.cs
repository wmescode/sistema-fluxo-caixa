using ControleLancamentos.Domain.Entities.ContasBancarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleLancamentos.ORM.Mapping
{
    public class ContaBancariaMapping : IEntityTypeConfiguration<ContaBancaria>
    {
        public void Configure(EntityTypeBuilder<ContaBancaria> builder)
        {            
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Numero)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(e => e.Agencia)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(e => e.Tipo)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(e => e.Saldo)
                .IsRequired();
            builder.HasMany(e => e.Lancamentos)
                .WithOne(e => e.Conta);                
        }
    }
}
