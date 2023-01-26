using MainBot;
using MainBot.Commands;
using MainBot.Database;
using MainBot.Logic;


IHost host = Host.CreateDefaultBuilder(args)
    
    .ConfigureServices(
        (hostContext, services) =>
        {
            
            services.AddSingleton<ApplicationContext>();
            services.AddHostedService<Bot>();
        })
    .Build();

await host.RunAsync();

