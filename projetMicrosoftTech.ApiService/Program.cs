using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            cat = new Cat
            {
                name = "Kohaku", age = 1, sex = "mâle",
                description = "Mon Kohaku, le plus beau et le plus parfait des chats existants en ce monde !"
            };
            context.Set<Cat>().Add(cat);
            context.Set<Photo>().Add(new Photo { cat = cat, photoUrl = ""});
            context.SaveChanges();
        }

    });
    
    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var cat = await context.Set<Cat>().FirstOrDefaultAsync(t => t.name == "Kohaku", cancellationToken);
        if (cat == null)
        {
            cat = new Cat
            {
                name = "Kohaku", age = 1, sex = "mâle",
                description = "Mon Kohaku, le plus beau et le plus parfait des chats existants en ce monde !"
            };
            context.Set<Cat>().Add(cat);
            context.Set<Photo>().Add(new Photo { cat = cat, photoUrl = ""});
            await context.SaveChangesAsync(cancellationToken);
        }
    });
});

// OpenAPI (Swagger simplifié)
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = builder.Configuration["Authentication:OIDC:Authority"];
        options.Audience = builder.Configuration["Authentication:OIDC:Audience"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            NameClaimType = "name",
            RoleClaimType = "role",
        };
    });

builder.Services.AddAuthorization();

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
app.MapGet("/api/cats", [Authorize] async (MyAppDbContext db) => 
    await db.Cat.Include(c => c.photos).ToListAsync());


app.MapPost("/api/cats", [Authorize] async (Cat item, MyAppDbContext db) =>
{
    db.Cat.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/api/cats/{item.id}", item);
});

app.MapPost("/api/cats/{catId}/photos", [Authorize] async (int catId, HttpRequest request, MyAppDbContext db) =>
{
    // Vérifier que le chat existe
    if (!await db.Cat.AnyAsync(c => c.id == catId))
        return Results.NotFound($"Chat avec l'id {catId} introuvable.");

    // Lire les fichiers depuis le multipart form
    if (!request.HasFormContentType)
        return Results.BadRequest("Le contenu doit être de type multipart/form-data.");

    var form = await request.ReadFormAsync();
    var file = form.Files.FirstOrDefault();
    if (file == null || file.Length == 0)
        return Results.BadRequest("Aucun fichier envoyé.");
    
    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    if (!allowedExtensions.Contains(extension))
        return Results.BadRequest("Type de fichier non autorisé.");
    
    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    if (!Directory.Exists(uploadsFolder))
        Directory.CreateDirectory(uploadsFolder);

    var filePath = Path.Combine(uploadsFolder, file.FileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var photo = new Photo
    {
        catId = catId,
        photoUrl = file.FileName
    };

    db.Photo.Add(photo);
    await db.SaveChangesAsync();

    return Results.Created($"/api/cats/{catId}/photos/{photo.id}", photo);
});

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.Run();