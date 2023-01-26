using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using MainBot.Attributes;
using MainBot.Database;
using MainBot.Database.Models;

namespace MainBot.Events;

[DiscordEvent("GuildMemberAdded")]
public class MemberAddEvent : BaseDiscordEvent
{
    private readonly ApplicationContext _db;
    
    public MemberAddEvent(IServiceScopeFactory scopeFactory) : base(scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        _db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    }
    
    public async Task RunEvent(DSharpPlus.DiscordClient ctx, DSharpPlus.EventArgs.GuildMemberAddEventArgs e)
    {
        _db.Users.Add(
            new User
            {
                Discriminant = e.Member.Discriminator,
                Name = e.Member.Username,
                DiscordId = e.Member.Id
            });
        await _db.SaveChangesAsync();
    }
}
