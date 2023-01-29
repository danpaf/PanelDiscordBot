using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using MainBot.Database;

public class TicketCommand : BaseCommandModule
{
    private readonly ApplicationContext _db;
    private readonly IServiceProvider _services;
    private readonly ComponentInteractionCreateEventArgs _contextevent;
    public TicketCommand(IServiceProvider services, ComponentInteractionCreateEventArgs contextevent)
    {
        _services = services;
        _db = services.GetRequiredService<ApplicationContext>();
        _contextevent = contextevent;  
    }
   
}
