using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Domain.Entities;

namespace Onion.Infrastructure.EntitiesConfiguration;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Numero).IsRequired();
        builder.Property(p => p.CEP).HasMaxLength(8).IsRequired();
        builder.Property(p => p.DataCriacao).IsRequired();
        builder.HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(c => c.ClienteId);
    }
}