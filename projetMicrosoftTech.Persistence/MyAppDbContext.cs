namespace projetMicrosoftTech.Persistence;
using Microsoft.EntityFrameworkCore;

public class MyAppDbContext : DbContext
{
    public DbSet<Cat> Cat { get; set; } = default!;
    public DbSet<Photo> Photo { get; set; } = default!;
    
    public DbSet<Favorite> Favorite { get; set; } = default!;

    public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options) { }
    
    // public MyAppDbContext() { }
    //
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //         optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;User ID=sa;Password=J8sJ1de*tx40f4wk{p-QdM;TrustServerCertificate=true;Initial Catalog=KohakuDB");
    //     }
    // }
    //
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new CatItemConfiguration().Configure(modelBuilder.Entity<Cat>());
        new PhotoItemConfiguration().Configure(modelBuilder.Entity<Photo>());
        new FavoriteItemConfiguration().Configure(modelBuilder.Entity<Favorite>());
        base.OnModelCreating(modelBuilder);
    }
}