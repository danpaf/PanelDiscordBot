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
    private readonly IConfiguration _configuration;
    public SlashCommands(IServiceProvider services,IConfiguration configuration)
    {
        _db = services.GetRequiredService<ApplicationContext>();
        _configuration = configuration;
    }
    //public ApplicationContext Db{ get; set; }

    [SlashCommand("ban", "Bans a user")]
    [SlashRequirePermissions(Permissions.BanMembers)]
    public async Task Ban(InteractionContext itx, [Option("user", "User to ban")] DiscordUser user,
        [Choice("None", 0)]
        [Choice("1 Day", 1)]
        [Choice("1 Week", 7)]
        [Option("deletedays", "Number of days of message history to delete")] long deleteDays = 0)
    {
        await itx.Guild.BanMemberAsync(user.Id, (int)deleteDays);
        await itx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Banned {user.Username}"));
        Funcs.DeleteCommandMessageItx(itx);
    }
 
    [SlashCommand("warn", "Warn a user")]
    [SlashRequirePermissions(Permissions.BanMembers)]
    public async Task Warn(InteractionContext itx, [Option("user", "User to warn")] DiscordUser member,[Option("reason", "Reason for removing warning")] string reason)
    {
        
      
        var user = _db.Users.Include(x => x.Events).FirstOrDefault(x => x.DiscordId == member.Id);
        var warnEvent = _db.Events.First(x => x.Name == "Warning");
        var warningCount = (from u in _db.Users
            join etu in _db.EventToUsers on u.Uid equals etu.UserUid
            where u.Uid == user.Uid && etu.EventUid == warnEvent.Uid
            select u).Count();
               
        if (warningCount == 3)
        {
            await itx.Guild.BanMemberAsync(member.Id,7,$"{member.Username} has been banned for 7 days for getting 3 warnings.");
            var entity = _db.EventToUsers.Where(x => x.UserUid == user.Uid && x.EventUid == warnEvent.Uid).ToList();
            if (entity != null)
            {
                _db.EventToUsers.RemoveRange(entity);
                _db.SaveChanges();
            }
            
            await itx.Guild.GetDefaultChannel().SendMessageAsync($"{member.Username} has been banned for 7 days for getting 3 warnings.");
        }
        _db.EventToUsers.Add(
            new EventToUser
            {
                UserUid = user.Uid,
                EventUid = warnEvent.Uid,
                Reason = reason
            });

        _db.SaveChanges();
        var emeb = Funcs.SendEmbedMessageItx(itx, "Warn", $"{member.Username}#{member.Discriminator}\nПричина: {reason} ", DateTime.Now);
        await itx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{emeb}"));
        Funcs.DeleteCommandMessageItx(itx);
        

    }
    
    [SlashCommand("unwarn", "Remove a warning from a user")]
    [SlashRequirePermissions(Permissions.BanMembers)]
    public async Task Unwarn(InteractionContext itx, [Option("user", "User to remove warning from")] DiscordUser member)
    {
        var user = _db.Users.Include(x => x.Events).FirstOrDefault(x => x.DiscordId == member.Id);
        var warnEvent = _db.Events.First(x => x.Name == "Warning");
        var warningToRemove = _db.EventToUsers.FirstOrDefault(x => x.UserUid == user.Uid && x.EventUid == warnEvent.Uid);
       
        if(warningToRemove != null)
        {
            _db.EventToUsers.Remove(warningToRemove);
            _db.SaveChanges();
            var emeb = Funcs.SendEmbedMessageItx(itx, "UnWarn", $"{member.Username}#{member.Discriminator}\n", DateTime.Now);
            
            await itx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{emeb}"));
        }
        else
        {
            await itx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{member.Username} doesn't have any warnings to remove"));
        }
        Funcs.DeleteCommandMessageItx(itx);
    }
        

    [ContextMenu(ApplicationCommandType.UserContextMenu, "User Menu")]
    public async Task UserMenu(ContextMenuContext ctx)
    {
        
    }
    
}   