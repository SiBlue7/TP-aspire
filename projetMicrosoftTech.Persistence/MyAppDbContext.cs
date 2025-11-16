namespace projetMicrosoftTech.Persistence;
using Microsoft.EntityFrameworkCore;

public class MyAppDbContext : DbContext
{
    public DbSet<Cat> Cat { get; set; } = default!;
    public DbSet<Photo> Photo { get; set; } = default!;
    public DbSet<Favorite> Favorite { get; set; } = default!;
    public DbSet<Adoption> Adoption { get; set; } = default!;

    public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new CatItemConfiguration().Configure(modelBuilder.Entity<Cat>());
        new PhotoItemConfiguration().Configure(modelBuilder.Entity<Photo>());
        new FavoriteItemConfiguration().Configure(modelBuilder.Entity<Favorite>());
        new AdoptionItemConfiguration().Configure(modelBuilder.Entity<Adoption>());
        base.OnModelCreating(modelBuilder);
    }
}