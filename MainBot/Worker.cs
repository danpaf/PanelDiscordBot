using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Menus;
using DSharpPlus.SlashCommands;
using MainBot.Commands;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.EventArgs;
using MainBot.Database;
using MainBot.Logic;

namespace MainBot;

public class Bot : BackgroundService
{
    private readonly DiscordClient _discord;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceCollection;
    public CommandsNextExtension Commands { get; set; }
    private readonly string[] _adminRoles;
    private readonly string[] _ownerRoles;

    public Bot(IServiceScopeFactory scopeFactory, IConfiguration configuration, IServiceProvider serviceCollection)
    {
       
        _discord = new DiscordClient(
            new DiscordConfiguration
            {
                Token = configuration["token"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
            });
        _serviceCollection = serviceCollection;
        var eventLogic = new EventLogic(_discord,scopeFactory);
        _configuration = configuration;
        eventLogic.RegisterEvents();
        _adminRoles = _configuration.GetSection("roles:adminRoles").Get<string[]>();
        _ownerRoles = _configuration.GetSection("roles:ownerRoles").Get<string[]>();

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       

        _discord.UseInteractivity(
            new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromSeconds(30)
            });


        var ccfg = new CommandsNextConfiguration
        {
            StringPrefixes = new[] { _configuration["prefix"] },
            
            EnableDms = true,

            EnableMentionPrefix = true,
            CaseSensitive = false,
            
        };


        var slash = _discord.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = _serviceCollection
        });
        slash.RegisterCommands<SlashCommands>();
        
        this.Commands = this._discord.UseCommandsNext(ccfg);
        this.Commands.RegisterCommands<Moderating>();
        
      
        

        _discord.ConnectAsync().GetAwaiter().GetResult();
        await Task.Delay(-1);   
    }

    /*private async Task OnMessageCreated(MessageCreateEventArgs e, List<DiscordApplicationCommand> commands)
    {
        foreach (var cmd in commands)
        {
            await cmd.Execute(e);
        }
    }*/
}