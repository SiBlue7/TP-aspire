using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace projetMicrosoftTech.Persistence;

public class AdoptionItemConfiguration
{
    public void Configure(EntityTypeBuilder<Adoption> builder)
    {
        builder.ToTable("adoption");
        builder.HasKey(t => t.id);
        builder.Property(t => t.comment).HasMaxLength(500);
        builder.Property(t => t.status).IsRequired();
        builder.Property(t => t.askedByUserId).IsRequired();
        builder.Property(t => t.catId).IsRequired();
    }
}