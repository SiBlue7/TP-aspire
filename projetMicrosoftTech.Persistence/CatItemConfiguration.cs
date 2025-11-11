using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace projetMicrosoftTech.Persistence;

public class CatItemConfiguration : IEntityTypeConfiguration<Cat>
{
    public void Configure(EntityTypeBuilder<Cat> builder)
    {
        builder.ToTable("cat");
        builder.HasKey(t => t.id);
        builder.Property(t => t.name).IsRequired().HasMaxLength(200);
        builder.Property(t => t.age).IsRequired();
        builder.Property(t => t.sex).IsRequired();
        builder.Property(t => t.description).IsRequired().HasMaxLength(1000);
        
        builder
            .HasMany(c => c.photos)
            .WithOne(p => p.cat)
            .HasForeignKey(p => p.catId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}