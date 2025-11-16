using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using projetMicrosoftTech.WebApp;
using projetMicrosoftTech.WebApp.Clients;
using projetMicrosoftTech.WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.AddServiceDefaults();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<TokenHandlers>();


builder.Services.AddHttpClient<ICatClient, CatClient>(client =>
{
    client.BaseAddress = new Uri("https+http://apiservice"); 
}).AddHttpMessageHandler<TokenHandlers>();

builder.Services.AddHttpClient<IFavoritesClient, FavoritesClient>(client =>
{
    client.BaseAddress = new Uri("https+http://apiservice");
}).AddHttpMessageHandler<TokenHandlers>();

builder.Services.AddHttpClient<IUserClient, UserClient>(client =>
{
    client.BaseAddress = new Uri("https+http://apiservice");
}).AddHttpMessageHandler<TokenHandlers>();

builder.Services.AddHttpClient<IAdoptionClient, AdoptionClient>(client =>
{
    client.BaseAddress = new Uri("https+http://apiservice");
}).AddHttpMessageHandler<TokenHandlers>();

builder.Services.AddAntiforgery();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["Authentication:OIDC:Authority"]; // la meme que pour l'API
        options.ClientId = builder.Configuration["Authentication:OIDC:ClientId"]; // le nom du client crée juste avant dans keycloak
        options.RequireHttpsMetadata = false;
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.CallbackPath = "/signin-oidc";
        options.SignedOutCallbackPath = "/signout-callback-oidc";
        options.UseTokenLifetime = true;
        options.MapInboundClaims = false;
        options.Scope.Add("api");
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "name", // Par défaut, le nom et le role sont mappés sur ces claims avec des noms différents
            RoleClaimType = "role",
        };
        options.ClaimActions.MapAll();
    });
builder.Services.ConfigureCookieOidc(CookieAuthenticationDefaults.AuthenticationScheme, "oidc");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/authentication/login", async context =>
{
    await context.ChallengeAsync("oidc", new AuthenticationProperties
    {
        RedirectUri = "/"
    });
});

app.MapGet("/authentication/logout", async context =>
{
    await context.SignOutAsync("oidc", new AuthenticationProperties
    {
        RedirectUri = "/"
    });

    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseAntiforgery();

app.Run();
