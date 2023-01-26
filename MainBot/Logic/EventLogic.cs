using System.Reflection;
using DSharpPlus;
using MainBot.Attributes;
using MainBot.Events;

namespace MainBot.Logic;

public class EventLogic
{
    private readonly DiscordClient _discord;
    private readonly IServiceScopeFactory _scopeFactory;

    public EventLogic(DiscordClient discord, IServiceScopeFactory scopeFactory)
    {
        _discord = discord;
        _scopeFactory = scopeFactory;
    }
    public void RegisterEvents()
    {
        
        var assembly = Assembly.GetExecutingAssembly();
        var classes = assembly.GetTypes().Where(t => t.IsClass && t.Namespace == "MainBot.Events" && !t.Name.StartsWith("Base")&& !t.Name.Contains("<"));
        foreach (var cls in classes)
        {
            var methodName = cls.GetCustomAttribute<DiscordEventAttribute>()!.MethodName;
            var eventInfo = _discord.GetType().GetEvent(methodName);

            var instance = (BaseDiscordEvent)Activator.CreateInstance(cls, _scopeFactory);
            var eventHandlerMethod = cls.GetMethod("RunEvent");
            var eventHandlerDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, instance, eventHandlerMethod);
            eventInfo.AddEventHandler(_discord, eventHandlerDelegate);
        }

    }
}
