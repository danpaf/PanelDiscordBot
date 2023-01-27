using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using MainBot.Database;
using MainBot.Database.Models;

namespace MainBot;

public class Funcs
{
    private readonly ApplicationContext _db;
    private readonly DiscordClient _discord;
   

    public Funcs(IServiceProvider services)
    {
        _db = services.GetRequiredService<ApplicationContext>();
        _discord = _discord;
    }

    
    
    public static ulong loggingChannelId = 1065686308091084860;
    public static async Task DeleteMessagesAsync(DiscordClient discord, DiscordChannel channel, int count)
    {
        var messages = await channel.GetMessagesAsync(count);
        await channel.DeleteMessagesAsync(messages);
    }
    
    public static async Task BanMemberAsync(DiscordClient discord, DiscordMember member, int daysOfMessagesToDelete = 0, string reason = "") {
        await member.BanAsync(daysOfMessagesToDelete, reason);
    }
    public static async Task StartActityBotStarting(DiscordClient client) {
        var start = new DiscordActivity
        {
            Name = "Bot Starting...",
            
            ActivityType = ActivityType.Playing
        };
        await client.UpdateStatusAsync(start);
        await Task.Delay(5000); 
        var started = new DiscordActivity
        {
            Name = "Bot Started",
            
            ActivityType = ActivityType.Playing
        };
        await client.UpdateStatusAsync(started);
        
    }
    public static async Task SendEmbedMessage(CommandContext ctx, string title, string description, DateTime Timestamp )
    {
        var embed = new DiscordEmbedBuilder
        {
            Title = title,
            Description = description,
            Timestamp = DateTime.Now,
            Color = DiscordColor.Green
        };

        await ctx.RespondAsync(embed: embed);
    }
    public static async Task SendEmbedMessageItx(InteractionContext itx, string title, string description, DateTime Timestamp )
    {
        var embed = new DiscordEmbedBuilder
        {
            Title = title,
            Description = description,
            Timestamp = DateTime.Now,
            Color = DiscordColor.Green
        };

        await itx.CreateResponseAsync(embed: embed);
    }
    public static async Task AutoDeleteMessage(DiscordMessage message) {
        await Task.Delay(5000); // waits for 5 seconds
        await message.DeleteAsync();
    }

    
    public static async Task DeleteLastMessages(DiscordChannel channel, int n) {
        var messages = await channel.GetMessagesAsync(n); // Get the last N messages in the channel
        foreach(var message in messages) {
            await message.DeleteAsync(); // Delete each message
        }
    }

    public static async Task DeleteCommandMessage(CommandContext ctx) {
        await ctx.Message.DeleteAsync();
    }
    public static async Task DeleteCommandMessageItx(InteractionContext ctx) {
        await ctx.Guild.DeleteAsync();
    }

    private static async Task LogMessage(DiscordClient client, DiscordMessage message, ulong logChannelId) {
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

    /*public static async Task CreateDropdownMenu(CommandContext ctx) {
        var interactivity = ctx.Client.GetInteractivity();

        var redRole = ctx.Guild.GetRole(1065731560525017138);
        var blueRole = ctx.Guild.GetRole(1065731577281253546);
        var greenRole = ctx.Guild.GetRole(1065731590476550165);

        var menu = new DiscordEmbedBuilder
        {
            Title = "Select a role",
            Description = "Please select one of the following roles:",
            Color = DiscordColor.Green
        };
        var redButton = new DiscordEmoji("🔴");
        var blueButton = new DiscordEmoji("🔵");
        var greenButton = new DiscordEmoji("🟢");

        var message = await ctx.RespondAsync(embed: menu);
        await message.CreateReactionAsync(redButton);
        await message.CreateReactionAsync(blueButton);
        await message.CreateReactionAsync(greenButton);

        var reactionResult = await interactivity.WaitForReactionAsync(
            x => x.Message == message &&
                 (x.Emoji == redButton || x.Emoji == blueButton || x.Emoji == greenButton)
            );

        var role = reactionResult.Emoji switch
        {
            DiscordEmoji.FromName(ctx.Client, "🔴") => redRole,
            DiscordEmoji.FromName(ctx.Client, "🔵") => blueRole,
            DiscordEmoji.FromName(ctx.Client, "🟢") => greenRole,
            _ => null
        };

        if (role != null)
        {
            await ctx.Member.GrantRoleAsync(role);
            await ctx.RespondAsync($"You have been granted the {role.Name} role!");
        }
        else
        {
            await ctx.RespondAsync("Invalid reaction, please try again.");
        }
    }*/
    //TODO: Пофиксить.

    public static async Task CreateSelectMenu(CommandContext ctx)
    {
        var interactivity = ctx.Client.GetInteractivity();

        var redRole = ctx.Guild.GetRole(1065731560525017138);
        var blueRole = ctx.Guild.GetRole(1065731577281253546);
        var greenRole = ctx.Guild.GetRole(1065731590476550165);

        var menu = new DiscordEmbedBuilder
        {
            Title = "Select a role",
            Description = "Please select one of the following roles:",
            Color = DiscordColor.Green
        };
        menu.AddField("1", "Red role", true);
        menu.AddField("2", "Blue role", true);
        menu.AddField("3", "Green role", true);

        var message = await ctx.RespondAsync(embed: menu);

        var response = await interactivity.WaitForMessageAsync(
            x => x.Author == ctx.User &&
                 (x.Content == "1" || x.Content == "2" || x.Content == "3"));

        var role = response.Result.Content switch
        {
            "1" => redRole,
            "2" => blueRole,
            "3" => greenRole,
            _ => null
        };

        if (role != null)
        {
            await ctx.Member.GrantRoleAsync(role);
            await ctx.RespondAsync($"You have been granted the {role.Name} role!");
        }
        else
        {
            await ctx.RespondAsync("Error");
        }
    }


   
}


