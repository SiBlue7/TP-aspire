using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace projetMicrosoftTech.Persistence;

public class CatItemConfiguration : IEntityTypeConfiguration<Cat>
{
    public void Configure(EntityTypeBuilder<Cat> builder)
    {
        builder.ToTable("cat");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.name).IsRequired().HasMaxLength(200);
    }
}