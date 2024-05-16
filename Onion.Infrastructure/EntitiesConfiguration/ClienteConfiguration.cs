using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Domain.Entities;

namespace Onion.Infrastructure.EntitiesConfiguration;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Documento).IsUnique();
        builder.Property(c => c.Documento).HasMaxLength(18).IsRequired();
        builder.Property(c => c.RazaoSocial).HasMaxLength(150).IsRequired();

        builder.HasMany(c => c.Pedidos)
            .WithOne(c => c.Cliente)
            .HasForeignKey(c => c.Id);
    }
}