using System.Text;
using DSharpPlus;
using Newtonsoft.Json;

namespace MainBot;

public class Bot : BackgroundService
{
    public static DiscordClient Discord;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
               
               
                Discord = new DiscordClient(new DiscordConfiguration
                {
                    Token = "",
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.All
                });
                Discord.MessageCreated += async (s,e) =>
                {
                    if (e.Message.Content.ToLower().StartsWith("ping")) 
                        await e.Message.RespondAsync("pong!");

                };

                await Discord.ConnectAsync();
                await Task.Delay(-1);
            
    }
}

