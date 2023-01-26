using DSharpPlus.EventArgs;

namespace MainBot;


public class DiscordApplicationCommand
{
    public string CommandTrigger { get; set; }
    public string Response { get; set; }

    public DiscordApplicationCommand(string commandTrigger, string response)
    {
        CommandTrigger = commandTrigger;
        Response = response;
    }

    public async Task Execute(MessageCreateEventArgs e)
    {
        if (!e.Message.Content.StartsWith(CommandTrigger))
            return;

        // command logic here
        await e.Message.RespondAsync(Response);
    }
}
