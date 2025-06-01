using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelPlanner.Domain.Entities;

namespace TravelPlanner.Infrastructure.Configuration;

public class RotaConfiguration : IEntityTypeConfiguration<Rota>
{
    public void Configure(EntityTypeBuilder<Rota> builder)
    {
        builder.ToTable("Rotas");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Origem).IsRequired().HasMaxLength(3);
        builder.Property(r => r.Destino).IsRequired().HasMaxLength(3);
        builder.Property(r => r.Valor).IsRequired().HasColumnType("decimal(18,2)");
    }
}