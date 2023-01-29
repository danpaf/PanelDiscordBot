using DSharpPlus.CommandsNext;
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
    .ConfigureLogging(
        logging =>
        {
            logging.ClearProviders();
            logging.AddDebug();
            //logging.AddConsole();
        })
    .Build();

await host.RunAsync();

