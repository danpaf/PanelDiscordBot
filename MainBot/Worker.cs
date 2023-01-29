using System.Text.RegularExpressions;
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
using MainBot.Database.Models;
using MainBot.Logic;
using Serilog;

namespace MainBot;

public class Bot : BackgroundService
{
   
    private readonly ApplicationContext _db;
    private readonly DiscordClient _discord;

    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceCollection;
    public CommandsNextExtension Commands { get; set; }
    private readonly string[] _adminRoles;
    private readonly string[] _ownerRoles;

    public Bot(IServiceScopeFactory scopeFactory, IConfiguration configuration, IServiceProvider serviceCollection)
    {
        _db = serviceCollection.GetRequiredService<ApplicationContext>();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();   
        var logFactory = new LoggerFactory().AddSerilog();
        _discord = new DiscordClient(
            new DiscordConfiguration
            {
                Token = configuration["token"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                LoggerFactory = logFactory
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
        this.Commands.RegisterCommands<MusicCommand>();
        
        this.Commands.CommandExecuted += async (s, e) =>
        {
            var cmd = e.Command;
            var user = e.Context;

            Log.Information("{Command} executed by {User}", cmd.Name, user.User.Username);
            var channel = await _discord.GetChannelAsync(Convert.ToUInt64(_configuration["logChannelId"]));
            
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Log")
                .WithDescription("Command executed by user")
                .AddField("Command", cmd.Name, true)
                .AddField("User", user.User.Mention, true)
                .WithTimestamp(DateTime.Now)
                .WithColor(DiscordColor.Green)
                .Build();
            channel.SendMessageAsync(embed: embed);
        };  
        slash.SlashCommandExecuted += async (s, e) =>
        {
            var cmd = e.Context;
            var user = e.Context.User;
            
            Log.Information("{Command} executed by {User} ({UserId})", cmd.CommandName, user.Username,user.Id);
            var channel = await _discord.GetChannelAsync(Convert.ToUInt64(_configuration["logChannelId"]));
            
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Log")
                .WithDescription("Command executed by user")
                .AddField("Command", cmd.CommandName, true)
                .AddField("User", user.Mention, true)
                .WithTimestamp(DateTime.Now)
                .WithColor(DiscordColor.Green)
                .Build();
            channel.SendMessageAsync(embed: embed);
        };     
        
        
    //Getting all server members
         async Task AddAllUsersToDatabase()
        {
            var guild = await _discord.GetGuildAsync(Convert.ToUInt64(_configuration["guild:guild_1"]));
            var users = await guild.GetAllMembersAsync();
    
            foreach (var user in users)
            {
                var dbUser = _db.Users.FirstOrDefault(x => x.DiscordId == user.Id);
                if (dbUser != null) continue;

                 dbUser = new User
                {
                    DiscordId = user.Id,
                    Name = user.Username,
                    Discriminant = user.Discriminator
                };

                _db.Users.Add(dbUser);
            }
            

            _db.SaveChanges();
        }
        
        
         AddAllUsersToDatabase();
         Funcs.StartActityBotStarting(_discord);
        _discord.ConnectAsync().GetAwaiter().GetResult();
        
        
        await Task.Delay(-1);   
    }


}