var builder = DistributedApplication.CreateBuilder(args);
var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
var db = sql.AddDatabase("KohakuDB");

var apiService = builder.AddProject<Projects.projetMicrosoftTech_ApiService>("apiservice")
    .WithReference(sql)
    .WaitFor(db);

builder.AddProject<Projects.projetMicrosoftTech_WebApp>("webapp")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
