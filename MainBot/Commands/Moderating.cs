using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace MainBot.Commands;

public class Moderating : BaseCommandModule
{
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

[Group("Admin")]
[Description("Administrative commands.")]
[Hidden]
[RequirePermissions(Permissions.ManageGuild)]
public class AdminCmds : BaseCommandModule
{

    [Command("sudo"), Description("Executes a command as another user."), Hidden, RequireOwner]
    public async Task Sudo(CommandContext ctx, [Description("Member to execute as.")] DiscordMember member,
        [RemainingText, Description("Command text to execute.")]
        string command)
    {
        await ctx.TriggerTypingAsync();
        
        var cmds = ctx.CommandsNext;
        var cmd = cmds.FindCommand(command, out var customArgs);
        var fakeContext = cmds.CreateFakeContext(member, ctx.Channel, command, ctx.Prefix, cmd, customArgs);

        await cmds.ExecuteCommandAsync(fakeContext);
    }

    [Command("ban"), Description("Executes a command as another user."), Hidden]
    public async Task Ban(CommandContext ctx, [Description("Member to execute as.")] DiscordMember member,
        [RemainingText, Description("Command text to execute.")]
        string command)

    {
        await ctx.TriggerTypingAsync();

        await Funcs.BanMemberAsync(ctx.Client, member, 7, "Violation of community guidelines");
    }
}