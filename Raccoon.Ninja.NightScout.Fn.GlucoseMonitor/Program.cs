using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddFilter("Raccoon.Ninja", LogLevel.Information);

builder.Build().Run();
