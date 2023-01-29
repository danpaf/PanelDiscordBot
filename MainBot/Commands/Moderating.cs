using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.EventArgs;
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
    [Hidden]
    [RequirePermissions(Permissions.BanMembers)]
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
    [Hidden]
    [RequirePermissions(Permissions.BanMembers)]
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
    [Hidden]
    
    [RequirePermissions(Permissions.KickMembers)]
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
    [Hidden]
    [RequirePermissions(Permissions.ManageMessages)]
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
    [Hidden]
    [Description(
        "Example ping command")] // this will be displayed to tell users what this command does when they invoke help
    [Aliases("pong")] // alternative names for the command
    public async Task Ping(CommandContext ctx) // this command takes no arguments
    {
        
        await ctx.TriggerTypingAsync();
        var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");
        await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        await Funcs.DeleteCommandMessage(ctx);
    }
    public async Task CreateTicket(ComponentInteractionCreateEventArgs e)
    {
        var response = new DiscordInteractionResponseBuilder()
            .WithTitle("Ticket Creation")
            .WithContent("You will be able to attach images in the thread after submitting the ticket.")
            .WithCustomId("ticketModal")
            .AddComponents(new TextInputComponent("Your Server Character", "serverCharacter",
                "Enter your server character name", required: true, min_length: 3))
            .AddComponents(new TextInputComponent("Ticket Title", "ticketTitle",
                "Enter a brief title for the ticket", required: true, min_length: 3))
            .AddComponents(new TextInputComponent("Ticket Description", "ticketDescription",
                "Enter a detailed description of the issue", required: true, min_length: 3, style: TextInputStyle.Paragraph));

        await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, response);
         
    }

    /*[Command("ticket")]
    public async Task CreationTicketCommand(ComponentInteractionCreateEventArgs contextevent)
    {
        var response = new DiscordInteractionResponseBuilder()
            .WithContent("Click the button to open the ticket modal")
            .AddComponents(new DiscordButtonComponent(ButtonStyle.Primary, "ticketModal", "ticket"))
            .Build();

        await contextevent.Message.CreateReactionAsync(response);
    }*/
    //TODO:см первый туду и доделать ЕТУ
    [Command("role")]
    [Hidden]
    public async Task RoleCommand(CommandContext ctx) {
        await Funcs.DeleteCommandMessage(ctx);
        await Funcs.CreateSelectMenu(ctx);
    }
    //TODO:Доделать StartActivity and StopActivity и права вызова!!!
    [Command("StartActivity"),RequireRoles(RoleCheckMode.Any,"DevOps")]
    [Hidden]
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
    [Hidden]
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


    [Command("createTicket")]
    [Hidden]
    [RequireRoles(RoleCheckMode.All,"DevOps")]
    public async Task CreateTicket(CommandContext ctx,ComponentInteractionCreateEventArgs e)
    {
        await Funcs.DeleteCommandMessage(ctx);
        var response = new DiscordInteractionResponseBuilder()
            .WithTitle("Ticket Creation")
            .WithContent("You will be able to attach screenshots in the thread after submitting the ticket.")
            .WithCustomId("ticketModal")
            .AddComponents(new TextInputComponent("Your server character name", "serverCharacterTicket",
                "Enter your server character name", required: true, min_length: 3))
            .AddComponents(new TextInputComponent("Problem title", "titleTicket",
                "Briefly describe the problem", required: true, min_length: 3))
            .AddComponents(new TextInputComponent("Problem description", "descriptionTicket",
                "Provide a detailed description of the problem", required: true, min_length: 3, style: TextInputStyle.Paragraph));

        await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal,response);
    }

    [Command("hjkdfgshjkjkdfskjdfsjkfdshjkdfhjkfhjkdshiurgejkertg")]
    [Hidden]
    [RequireRoles(RoleCheckMode.All,"DevOps")]
    public async Task Secret(CommandContext ctx)
    {
        await Funcs.DeleteCommandMessage(ctx);
        await ctx.RespondAsync($"https://media.discordapp.net/attachments/1053038297976414248/1068236573054861342/image.png?width=275&height=674");

    }
}



