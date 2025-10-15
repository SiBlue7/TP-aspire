var builder = DistributedApplication.CreateBuilder(args);
var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
var db = sql.AddDatabase("KohakuDB");

builder.AddProject<Projects.projetMicrosoftTech_ApiService>("apiservice")
    .WithReference(sql)
    .WaitFor(db);

builder.Build().Run();
