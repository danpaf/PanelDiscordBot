using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace MainBot;

public class Funcs : BackgroundService
{
    public async Task DeleteMessagesAsync(DiscordClient client, DiscordChannel channel, int count)
    {
        var messages = await channel.GetMessagesAsync(count);
        await channel.DeleteMessagesAsync(messages);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Funcs loaded");
    }
}
