using DSharpPlus;
using MainBot;

IHost host = Host.CreateDefaultBuilder(args)
    
    .ConfigureServices(services => { services.AddHostedService<Bot>(); })
    .Build();

await host.RunAsync();

