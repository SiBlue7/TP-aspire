namespace projetMicrosoftTech.Persistence;
using Microsoft.EntityFrameworkCore;

public class MyAppDbContext : DbContext
{
    public DbSet<Cat> Cat { get; set; } = default!;
    public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new CatItemConfiguration().Configure(modelBuilder.Entity<Cat>());
        base.OnModelCreating(modelBuilder);
    }
}