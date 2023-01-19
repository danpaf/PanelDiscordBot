using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MainBot.Commands;
using Newtonsoft.Json;
using DSharpPlus.Menus;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace MainBot;


public class Bot : BackgroundService
{
    private readonly DiscordClient _discord;
    private readonly IConfiguration _configuration;
    public CommandsNextExtension Commands { get; set; }
    
    public Bot(IServiceScopeFactory scopeFactory
        ,IConfiguration configuration)
    {
        var scope = scopeFactory.CreateScope();
        _discord = scope.ServiceProvider.GetRequiredService<DiscordClient>();
            _configuration = configuration;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
       
        Dictionary<int, string> roles = new Dictionary<int, string>();
        roles = new Dictionary<int, string>()
        {
            [1] = "DEV",
            [2] = "admin",
            [3] = "help"
        };
        
               
               
                var ccfg = new CommandsNextConfiguration
                {
                    
                    StringPrefixes = new[] {_configuration["prefix"]},

                    
                    EnableDms = true,
                    
                    EnableMentionPrefix = true
                };
                this.Commands = this._discord.UseCommandsNext(ccfg);
                this.Commands.RegisterCommands<Moderating>();
                this.Commands.RegisterCommands<AdminCmds>();

                await _discord.ConnectAsync();
                await Task.Delay(-1);
                
    }
}

public struct ConfigJson
{
    [JsonProperty("token")]
    public string Token { get; private set; }

    [JsonProperty("prefix")]
    public string CommandPrefix { get; private set; }
}