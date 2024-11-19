var builder = DistributedApplication.CreateBuilder(args);
var container = builder.AddContainer("web", "nginx", "1.27.2")
    .WithEndpoint(targetPort: 80);

builder.Build().Run();
