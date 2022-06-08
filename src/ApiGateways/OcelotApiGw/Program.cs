using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
});
builder.Host.ConfigureLogging((hostingContext, loggingBuilder) =>
{
    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

builder.Services.AddOcelot()
    .AddCacheManager(opt => opt.WithDictionaryHandle());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseOcelot();

app.Run();
