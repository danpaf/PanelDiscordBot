using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

namespace MainBot.Events;

public class BaseDiscordEvent
{
    protected readonly IServiceScopeFactory ScopeFactory;
    public BaseDiscordEvent(IServiceScopeFactory scopeFactory)
    {
        ScopeFactory = scopeFactory;
    }

}
