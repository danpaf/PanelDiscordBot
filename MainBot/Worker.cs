using DSharpPlus;
using DSharpPlus.CommandsNext;
using MainBot.Commands;

namespace MainBot;

public class Bot : BackgroundService
{
    private readonly DiscordClient _discord;
    private readonly IConfiguration _configuration;
    public CommandsNextExtension Commands { get; set; }

    public Bot(IServiceScopeFactory scopeFactory
        , IConfiguration configuration)
    {
        _discord = new DiscordClient(new DiscordConfiguration
        {
            Token = configuration["token"],
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All
        });

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
            StringPrefixes = new[] { _configuration["prefix"] },


            EnableDms = true,

            EnableMentionPrefix = true
        };
        this.Commands = this._discord.UseCommandsNext(ccfg);
        this.Commands.RegisterCommands<Moderating>();

        await _discord.ConnectAsync();
        await Task.Delay(-1);
    }
}