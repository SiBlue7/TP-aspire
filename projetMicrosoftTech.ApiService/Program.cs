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
                name = "Kohaku", age = 1, sex = "mâle", createdByUserId = "fake-id",
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
                name = "Kohaku", age = 1, sex = "mâle", createdByUserId = "fake-id",
                description = "Mon Kohaku, le plus beau et le plus parfait des chats existants en ce monde !"
            };
            context.Set<Cat>().Add(cat);
            context.Set<Photo>().Add(new Photo { cat = cat, photoUrl = "Kohaku.jpeg"});
            var cat2 = new Cat
            {
                name = "Lilo", age = 1, sex = "femelle", createdByUserId = "22222222-2222-2222-2222-222222222222",
                description = "Petite Lilo, mignonne et caline !"
            };
            context.Set<Cat>().Add(cat2);
            context.Set<Photo>().Add(new Photo { cat = cat2, photoUrl = "lilo.png"});
            var cat3 = new Cat
            {
                name = "Cookie", age = 3, sex = "mâle", createdByUserId = "22222222-2222-2222-2222-222222222222",
                description = "Cookie attend impatiemment d'être adopté !"
            };
            context.Set<Cat>().Add(cat3);
            context.Set<Photo>().Add(new Photo { cat = cat3, photoUrl = "default.png"});
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
builder.Services.AddControllers();

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
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.Run();