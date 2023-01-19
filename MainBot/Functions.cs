using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace MainBot;

public static class Funcs
{
    public static async Task DeleteMessagesAsync(DiscordClient discord, DiscordChannel channel, int count)
    {
        var messages = await channel.GetMessagesAsync(count);
        await channel.DeleteMessagesAsync(messages);
    }
    
    public static async Task BanMemberAsync(DiscordClient discord, DiscordMember member, int daysOfMessagesToDelete = 0, string reason = "") {
        await member.BanAsync(daysOfMessagesToDelete, reason);
    }
    
    
}
