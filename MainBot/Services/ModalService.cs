using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;

namespace MainBot.Services;

public class ModalService
{
    private readonly IConfiguration _configuration;
    public ModalService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task ModalButtonPressed(DiscordClient sender, ComponentInteractionCreateEventArgs e)
    {
        var response = new DiscordInteractionResponseBuilder()
            .WithTitle("Создание тикета о репорте")
            .WithContent("Вы сможете прикрепить графические вложения в ветке, после отправки тикета")
            .WithCustomId("modalForBugReports")
            .AddComponents(new TextInputComponent("Ваш ник на сервере", "serverCharacterModalForBugReports",
                "Введите ваш никнейм на сервере", required: true, min_length: 3))
            .AddComponents(new TextInputComponent("Заголовок проблемы", "headerModalForBugReports",
                "Кратко опишите проблему", required: true, min_length: 3))
            .AddComponents(new TextInputComponent("Описание проблемы", "descModalForBugReports",
                "Подробно опишите проблему", required: true, min_length: 3, style: TextInputStyle.Paragraph));

        await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, response);
    }
    public async Task ModalSubmitted(DiscordClient sender, ModalSubmitEventArgs e)
    {
        try
        {
            if (e.Interaction.Data.CustomId != "modalForBugReports") return;
         
            var guild = await sender.GetGuildAsync(Convert.ToUInt64(_configuration["guild:guild_1"]));
            var channel = guild.GetChannel(Convert.ToUInt64("1070802365432741909"));

            var threadAsync = await channel.CreateThreadAsync(e.Values.Values.ToList()[1], AutoArchiveDuration.ThreeDays,
                ChannelType.PrivateThread);

            await threadAsync.SendMessageAsync(
                $"**Баг репорт от:** ({e.Values.Values.ToList()[0]}) <@{e.Interaction.User.Id}>\n\n>>> {e.Values.Values.ToList()[2]}");
            

            var notifyChannel = guild.GetChannel(Convert.ToUInt64("1065686308091084860"));
            await notifyChannel.SendMessageAsync($"<@&1023623900996837476> Поступил новый репорт. Проверьте канал <#{threadAsync.Id}>");

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }
        catch (BadRequestException exception)
        {
            Console.WriteLine(exception.JsonMessage);
            Console.WriteLine(exception.Errors);
        }
    }
   
    

}
