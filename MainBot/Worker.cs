using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using MainBot.Commands;
using Newtonsoft.Json;

namespace MainBot;


public class Bot : BackgroundService
{
    public DiscordClient Discord;
    public CommandsNextExtension Commands { get; set; }

    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Dictionary<int, string> roles = new Dictionary<int, string>();
        roles = new Dictionary<int, string>()
        {
            [1] = "DEV",
            [2] = "admin",
            [3] = "help"
        };
        
               
                Discord = new DiscordClient(new DiscordConfiguration
                {
                    Token = "",
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.All
                });
                var ccfg = new CommandsNextConfiguration
                {
                    // let's use the string prefix defined in config.json
                    StringPrefixes = new[] {"!"},

                    // enable responding in direct messages
                    EnableDms = true,

                    // enable mentioning the bot as a command prefix
                    EnableMentionPrefix = true
                };
                this.Commands = this.Discord.UseCommandsNext(ccfg);
                this.Commands.RegisterCommands<Moderating>();

                await Discord.ConnectAsync();
                await Task.Delay(-1);
            
    }
}

