using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace MainBot.Commands;

public class Moderating : BaseCommandModule
{
    [Command("ban")]
    public async Task BanUser(CommandContext ctx, [Description("The user to ban")] DiscordMember member, [RemainingText, Description("The reason for the ban")] string reason)
    {
        if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.BanMembers))
        {
            await ctx.RespondAsync("Не достаточно прав для бана.");
            return;
        }
        if (reason == null)
        {
            reason = "Без причины";
        }
        await member.BanAsync(reason: reason);
        await Funcs.SendEmbedMessage(ctx, "Бан", $"{member.Username}#{member.Discriminator}\nПричина: {reason} ",DateTime.Now);
    }
    

    [Command("unban")]
    public async Task UnbanUser(CommandContext ctx, [Description("The user to unban")] ulong userId, [RemainingText, Description("The reason for the unban")] string reason)
    {
        if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.BanMembers))
        {
            await ctx.RespondAsync("Не достаточно прав для бана.");
            return;
        }
        var ban = await ctx.Guild.GetBanAsync(userId);
        if (ban == null)
        {
            await ctx.RespondAsync("The specified user is not banned.");
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
        if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.KickMembers))
        {
            await ctx.RespondAsync("Не достаточно прав для кика.");
            return;
        }
        if (reason == null)
        {
            reason = "Без причины";
        }
        await member.RemoveAsync();
        await Funcs.SendEmbedMessage(ctx, "Кик", $"{member.Username}#{member.Discriminator}\nПричина: {reason}",DateTime.Now);

    }
    
    [Command("ping")] // let's define this method as a command
    [Description(
        "Example ping command")] // this will be displayed to tell users what this command does when they invoke help
    [Aliases("pong")] // alternative names for the command
    public async Task Ping(CommandContext ctx) // this command takes no arguments
    {
        await ctx.TriggerTypingAsync();
        var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");
        await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
    }


    [Command("greet"), Description("Says hi to specified user."), Aliases("sayhi", "say_hi")]
    public async Task
        Greet(CommandContext ctx,
            [Description("The user to say hi to.")]
            DiscordMember member) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
    {
        // note the [Description] attribute on the argument.
        // this will appear when people invoke help for the
        // command.

        // let's trigger a typing indicator to let
        // users know we're working
        await ctx.TriggerTypingAsync();
        // let's make the message a bit more colourful
        var emoji = DiscordEmoji.FromName(ctx.Client, ":wave:");
        // and finally, let's respond and greet the user.
        await ctx.RespondAsync($"{emoji} Hello, {member.Mention}!");
    }
}

