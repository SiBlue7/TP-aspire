var builder = DistributedApplication.CreateBuilder(args);
var sql = builder.AddSqlServer("sql", port: 1433)
    .WithLifetime(ContainerLifetime.Persistent);
var db = sql.AddDatabase("KohakuDB");

var apiService = builder.AddProject<Projects.projetMicrosoftTech_ApiService>("apiservice")
    .WithReference(sql)
    .WaitFor(db);

builder.AddProject<Projects.projetMicrosoftTech_WebApp>("webapp")
    .WithReference(apiService)
    .WaitFor(apiService);

var keycloak = builder.AddKeycloak("keycloak", 8090)
    .WithBindMount("./keycloak", "/opt/keycloak/data/import")
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithEnvironment("KEYCLOAK_IMPORT", "/opt/keycloak/data/import/realm-export.json")
    .WithLifetime(ContainerLifetime.Persistent);

builder.Build().Run();
