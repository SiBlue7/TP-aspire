using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace projetMicrosoftTech.Persistence;

public class FavoriteItemConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("favorite");
        builder.HasKey(f => f.id);
        builder.Property(t => t.catId).IsRequired();
        builder.Property(f => f.userId)
            .IsRequired()
            .HasMaxLength(200);
    }
}