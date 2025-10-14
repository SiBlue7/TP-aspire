var builder = DistributedApplication.CreateBuilder(args);
var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
var initScriptPath = Path.Join(Path.GetDirectoryName(typeof(Program).Assembly.Location), "init.sql");
var db = sql.AddDatabase("KohakuDB")
    .WithCreationScript(File.ReadAllText(initScriptPath));

builder.AddProject<Projects.projetMicrosoftTech_ApiService>("apiservice")
    .WithReference(sql)
    .WaitFor(db);

builder.Build().Run();
