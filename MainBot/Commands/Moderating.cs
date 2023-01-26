using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.Lavalink;
using MainBot.Enums;
using MainBot.Resources;


namespace MainBot.Commands;

public class Moderating : BaseCommandModule
{
    public static IConfiguration _configuration;
    private readonly string[] _adminRoles;
    private readonly string[] _ownerRoles;

    /*public Moderating(IConfiguration configuration)
    {
        _configuration = configuration;
        _adminRoles = _configuration.GetSection("roles:adminRoles").Get<string[]>();
        _ownerRoles = _configuration.GetSection("roles:ownerRoles").Get<string[]>();
    }*/
    private async Task DeleteCommandMessage(CommandContext ctx) {
        await ctx.Message.DeleteAsync();
    }
    [Command("ban")]
    public async Task BanUser(CommandContext ctx, [Description("The user to ban")] DiscordMember member, [RemainingText, Description("The reason for the ban")] string reason)
    {
        await Funcs.DeleteCommandMessage(ctx);
        if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.BanMembers))
        {
          DiscordMessage message =  await ctx.RespondAsync("Не достаточно прав для бана.");
          await Funcs.AutoDeleteMessage(message);
            return;
        }
        if (reason == null)
        {
            reason = "Без причины.";
        }
        await member.BanAsync(reason: reason);
        await Funcs.SendEmbedMessage(ctx, "Бан", $"{member.Username}#{member.Discriminator}\nПричина: {reason} ",DateTime.Now);
    }
    

    [Command("unban")]
    public async Task UnbanUser(CommandContext ctx, [Description("The user to unban")] ulong userId, [RemainingText, Description("The reason for the unban")] string reason)
    {
        await Funcs.DeleteCommandMessage(ctx);
        if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.BanMembers))
        {
            DiscordMessage message =  await ctx.RespondAsync("Не достаточно прав для разбана.");
            await Funcs.AutoDeleteMessage(message);
            return;
        }
        var ban = await ctx.Guild.GetBanAsync(userId);
        if (ban == null)
        {
            DiscordMessage message = await ctx.RespondAsync("The specified user is not banned.");
            await Funcs.AutoDeleteMessage(message);
            return;
        }
        if (reason == null)
        {
            reason = "Без причины";
        }
        await ctx.Guild.UnbanMemberAsync(userId);
        //await ctx.RespondAsync($"{ban.User.Username}#{ban.User.Discriminator} has been unbanned for the reason: {reason}");
        await Funcs.SendEmbedMessage(ctx, "Разбан", $"{ban.User.Username}#{ban.User.Discriminator}\nПричина: {reason}",DateTime.Now);
    }
    [Command("kick")]
    [Description("kicks member ferom guild")]
    public async Task kick(CommandContext ctx, DiscordMember member,[RemainingText, Description("The reason for the unban")] string reason)
    {
        await Funcs.DeleteCommandMessage(ctx);
        if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.KickMembers))
        {
            
            DiscordMessage message = await ctx.RespondAsync("Не достаточно прав для кика.");
            await Funcs.AutoDeleteMessage(message);
            return;
        }
        if (reason == null)
        {
            reason = "Без причины";
        }
        await member.RemoveAsync();
        await Funcs.SendEmbedMessage(ctx, "Кик", $"{member.Username}#{member.Discriminator}\nПричина: {reason}",DateTime.Now);

    }
    [Command("clear")]
    [Description("kicks member ferom guild")]
    public async Task ClearMes(CommandContext ctx, int n) {
        await Funcs.DeleteCommandMessage(ctx);
        if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.KickMembers))
        {
            DiscordMessage message = await ctx.RespondAsync("Не достаточно прав для кика.");
            await Funcs.AutoDeleteMessage(message);
            return;
        }
        var messages = await ctx.Channel.GetMessagesAsync(n); // Get the last N messages in the channel
        foreach(var message in messages)
        {
            await Task.Delay(500);
            await message.DeleteAsync(); // Delete each message
        }
    }
    private async Task LogMessage(DiscordClient client, DiscordMessage message, ulong logChannelId = 1065686308091084860 ) {
        var logChannel = await client.GetChannelAsync(logChannelId);
        var embed = new DiscordEmbedBuilder
        {
            Title = $"Message from {message.Author.Username} in {message.Channel.Name}",
            Description = message.Content,
            Timestamp = message.Timestamp,
            Color = DiscordColor.Green,
            Author = new DiscordEmbedBuilder.EmbedAuthor
            {
                IconUrl = message.Author.AvatarUrl,
                Name = message.Author.Username
            },
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "Timestamp"
            }
        };
        if(message.Attachments.Count > 0)
        {
            embed.ImageUrl = message.Attachments.First().Url;
        }
        await logChannel.SendMessageAsync(embed: embed);
    }


    [Command("ping")] // let's define this method as a command
    [Description(
        "Example ping command")] // this will be displayed to tell users what this command does when they invoke help
    [Aliases("pong")] // alternative names for the command
    public async Task Ping(CommandContext ctx) // this command takes no arguments
    {
        await Funcs.DeleteCommandMessage(ctx);
        await ctx.TriggerTypingAsync();
        var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");
        await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
    }

    [Command("role")]
    public async Task RoleCommand(CommandContext ctx) {
        await Funcs.DeleteCommandMessage(ctx);
        await Funcs.CreateSelectMenu(ctx);
    }
    //TODO:Доделать StartActivity and StopActivity и права вызова!!!
    [Command("StartActivity"),RequireRoles(RoleCheckMode.Any,"DevOps")]
    public async Task StartActivity(CommandContext ctx,string name)
    {
        await Funcs.DeleteCommandMessage(ctx);
        if (name == null)
        {
            name = "Работает и славно";
        }
        var activity = new DiscordActivity
        {
            Name = name,
            
            ActivityType = ActivityType.Playing
        };
        await ctx.Client.UpdateStatusAsync(activity);
        
    }
    [Command("StopActivity")]
    [RequireRoles(RoleCheckMode.All,"DevOps")]
    public async Task StopActivity(CommandContext ctx) {
        await Funcs.DeleteCommandMessage(ctx);
        var activity = new DiscordActivity
        {
            Name = " ",
            
            ActivityType = ActivityType.Playing
        };
        await ctx.Client.UpdateStatusAsync(null);
        
    }
    [Command("hjkdfgshjkjkdfskjdfsjkfdshjkdfhjkfhjkdshiurgejkertg")]
    [RequireRoles(RoleCheckMode.All,"DevOps")]
    public async Task Secret(CommandContext ctx)
    {
        await Funcs.DeleteCommandMessage(ctx);
        await ctx.RespondAsync($"https://media.discordapp.net/attachments/1053038297976414248/1068236573054861342/image.png?width=275&height=674");

    }
}



