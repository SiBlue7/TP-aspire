using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace projetMicrosoftTech.Persistence;

public class PhotoItemConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("photo");
        builder.HasKey(t => t.id);
        builder.Property(t => t.catId).IsRequired();
        builder.Property(t => t.photoUrl).IsRequired();
    }
}