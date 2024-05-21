using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Domain.Entities;

namespace Onion.Infrastructure.EntitiesConfiguration;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Nome).HasMaxLength(80).IsRequired();
        builder.Property(p => p.Valor).HasPrecision(18, 2).IsRequired();
        
        builder.HasData(
            new Produto(1, "CELULAR", 1000),
            new Produto(2, "NOTEBOOK", 3000),
            new Produto(3, "TELEVISÃO", 5000)
        );
    }
}