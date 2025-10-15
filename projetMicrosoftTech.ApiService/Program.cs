using Microsoft.EntityFrameworkCore;
using projetMicrosoftTech.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Aspire injectera la bonne chaîne de connexion à "sql"
builder.AddServiceDefaults();
builder.Services.AddDbContext<MyAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql"));
    
    options.UseSeeding((context, _) =>
    {
        var cat = context.Set<Cat>().FirstOrDefault(t => t.name == "Kohaku");
        if (cat == null)
        {
            context.Set<Cat>().Add(new Cat { name = "Kohaku"});
            context.SaveChanges();
        }

    });
    
    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var cat = await context.Set<Cat>().FirstOrDefaultAsync(t => t.name == "Kohaku", cancellationToken);
        if (cat == null)
        {
            context.Set<Cat>().Add(new Cat { name = "Kohaku"});
            await context.SaveChangesAsync(cancellationToken);
        }
    });
});

// OpenAPI (Swagger simplifié)
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();
        await context.Database.EnsureCreatedAsync();
    }
}

app.UseHttpsRedirection();

// --- Minimal API endpoints ---
app.MapGet("/api/cats", async (MyAppDbContext db) => await db.Cat.ToListAsync());

app.MapPost("/api/cats", async (Cat item, MyAppDbContext db) =>
{
    db.Cat.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/api/cats/{item.Id}", item);
});

// --- Exemple météo ---
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}