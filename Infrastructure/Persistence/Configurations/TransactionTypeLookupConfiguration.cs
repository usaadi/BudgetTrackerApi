using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Persistence.Configurations;

public class TransactionTypeLookupConfiguration : IEntityTypeConfiguration<TransactionTypeLookup>
{
    public void Configure(EntityTypeBuilder<TransactionTypeLookup> builder)
    {
        builder.HasKey(e => e.Id).HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.None);

        builder.Property(t => t.Name)
            .HasMaxLength(50);
    }
}
