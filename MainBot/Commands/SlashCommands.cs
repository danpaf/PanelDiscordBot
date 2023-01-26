using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands.Attributes;
using MainBot.Database;
using MainBot.Database.Models;
using MainBot.Extensions;
using Microsoft.EntityFrameworkCore;


namespace MainBot.Commands;


public class SlashCommands : ApplicationCommandModule
{
    
    private readonly ApplicationContext _db;

    public SlashCommands(IServiceProvider services)
    {
        _db = services.GetRequiredService<ApplicationContext>();
    }
    //public ApplicationContext Db{ get; set; }

    [SlashCommand("ban", "Bans a user")]
    [SlashRequirePermissions(Permissions.BanMembers)]
    public async Task Ban(InteractionContext ctx, [Option("user", "User to ban")] DiscordUser user,
        [Choice("None", 0)]
        [Choice("1 Day", 1)]
        [Choice("1 Week", 7)]
        [Option("deletedays", "Number of days of message history to delete")] long deleteDays = 0)
    {
        await ctx.Guild.BanMemberAsync(user.Id, (int)deleteDays);
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Banned {user.Username}"));
    }
    [SlashCommand("warn", "Warn a user")]
    [SlashRequirePermissions(Permissions.BanMembers)]
    public async Task Warn(InteractionContext itx, [Option("user", "User to warn")] DiscordUser member)
    {
        
        /*public static async Task<int> AddWarningAsync(this User member)
    {
        var warnings = _userWarnings.AddOrUpdate(member.Id, 1, (key, oldValue) => oldValue + 1);
        await member.SendMessageAsync("You have been warned. You now have " + warnings + " warnings.");
        if (warnings >= 3)
        {
            await member.BanAsync(7,"Reached the maximum number of warnings");
            _userWarnings.TryRemove(member.Id, out _);
            await member.Guild.GetDefaultChannel().SendMessageAsync($"{member.Username} has been banned for {_banDuration.Days} days for getting 3 warnings.");
        }
        return warnings;
    }*/
        var user = _db.Users.Include(x => x.Events).FirstOrDefault(x => x.DiscordId == member.Id);
        var warnEvent = _db.Events.First(x => x.Name == "Warning");
        var warningCount = (from u in _db.Users
            join etu in _db.EventToUsers on u.Uid equals etu.UserUid
            where u.Uid == user.Uid && etu.EventUid == warnEvent.Uid
            select u).Count();
        if (warningCount >= 3)
        {
            await itx.Guild.BanMemberAsync(member.Id,7,$"{member.Username} has been banned for 7 days for getting 3 warnings.");
            /*var entity = _db.EventToUsers.First(x => x.UserUid == "useruid");
            if (entity != null)
            {
                _db.Users.Remove(entity);
                _db.SaveChanges();
            }*/
            //TODO:ДОПИСАТЬ УДАЛЕНИЕ ИЗ БД
            await itx.Guild.GetDefaultChannel().SendMessageAsync($"{member.Username} has been banned for 7 days for getting 3 warnings.");
        }
        _db.EventToUsers.Add(
            new EventToUser
            {
                UserUid = user.Uid,
                EventUid = warnEvent.Uid
            });

        _db.SaveChanges();
        await itx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Banned "));

    }

    [SlashCommand("ping", "Checks the latency between the bot and it's database. Best used to see if the bot is lagging.")]
    public async Task Ping(InteractionContext context) => await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
    {
        Content = $"Pong! Database latency is {context.Client.Ping}ms."
    });


    [ContextMenu(ApplicationCommandType.UserContextMenu, "User Menu")]
    public async Task UserMenu(ContextMenuContext ctx)
    {
        
    }
    
}   