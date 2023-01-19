using DSharpPlus;
using MainBot;
using Nefarius.DSharpPlus.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    
    .ConfigureServices(
        (hostContext, services) =>
        {
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = hostContext.Configuration["token"],
                TokenType = TokenType.Bot,
                
            });
            discord.ConnectAsync().GetAwaiter().GetResult();
            services.AddSingleton(discord);
            services.AddHostedService<Bot>();
        })
    .Build();

await host.RunAsync();

