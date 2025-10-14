var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.projetMicrosoftTech_ApiService>("apiservice");

builder.Build().Run();
