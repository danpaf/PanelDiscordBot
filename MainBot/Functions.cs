using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace MainBot;

public static class Funcs
{
    public static ulong loggingChannelId = 1065686308091084860;
    public static async Task DeleteMessagesAsync(DiscordClient discord, DiscordChannel channel, int count)
    {
        var messages = await channel.GetMessagesAsync(count);
        await channel.DeleteMessagesAsync(messages);
    }
    
    public static async Task BanMemberAsync(DiscordClient discord, DiscordMember member, int daysOfMessagesToDelete = 0, string reason = "") {
        await member.BanAsync(daysOfMessagesToDelete, reason);
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
    private static async Task LogMessage(DiscordClient client, DiscordMessage message, ulong logChannelId) {
        var logChannel = await client.GetChannelAsync(logChannelId);
        var embed = new DiscordEmbedBuilder
        {
            Title = $"Message from {message.Author.Username} in {message.Channel.Name}",
            Description = message.Content,
            Timestamp = message.Timestamp,
        };
        await logChannel.SendMessageAsync(embed: embed);
    }

    
}
