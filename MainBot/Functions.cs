using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using MainBot.Database;
using System.Drawing;
using System;
using MainBot.Database.Models;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace MainBot;

public class Funcs
{
    private readonly ApplicationContext _db;
    private readonly DiscordClient _discord;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceCollection;
   

    public Funcs(IServiceProvider services,IConfiguration configuration,DiscordClient discord)
    {
        _db = services.GetRequiredService<ApplicationContext>();
        _discord = discord;
        _configuration = configuration;
    }

    
    
    public static ulong loggingChannelId = 1065686308091084860;
    public static async Task DeleteMessagesAsync(DiscordClient discord, DiscordChannel channel, int count)
    {
        var messages = await channel.GetMessagesAsync(count);
        await channel.DeleteMessagesAsync(messages);
    }
    public static MemoryStream BitmapToStream(Bitmap bitmap)
    {
        var stream = new MemoryStream();
        bitmap.Save(stream,ImageFormat.Png);
        stream.Position = 0;
        return stream;
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
        await Task.Delay(5000);
        await ctx.Message.DeleteAsync();
    }
    public static async Task DeleteCommandMessageItx(InteractionContext ctx)
    {
        await Task.Delay(5000);
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
    public static async Task ModalInfoMessage(CommandContext ctx)
    {
        DiscordEmbedBuilder embed = new DiscordEmbedBuilder
        {
            Title = "Добро пожаловать на тут имя !",
            Description =
                "Это место, где вы можете исследовать, строить и играть с другими людьми в виртуальном мире. Наш сервер предлагает большие возможности для совместной игры и творчества, а также множество уникальных карт и модификаций, чтобы ваш опыт игры был незабываемым. Присоединяйтесь к нам и давайте начнем нашу приключение вместе!",
            Color = new DiscordColor("#7289DA")
        };
        
        var builder = new DiscordMessageBuilder()
            .WithEmbed(embed)
            
            .AddComponents(new DiscordComponent[]
            {
                
                new DiscordLinkButtonComponent("https://ya.ru/", "Наш сайт!"),
                new DiscordLinkButtonComponent("https://ya.ru/", "ТГ канал!"),
                new DiscordLinkButtonComponent("https://ya.ru/", "Ссылка на лаунчер")
            });

        await ctx.RespondAsync(builder);
        
    }
    
    /*public static async Task GenerateCards(CommandContext ctx)
    {
        DiscordEmbedBuilder embed = new DiscordEmbedBuilder
        {
            Title = "Бинго!",
            Description =
                "Это твои карты!",
            Color = new DiscordColor("#7289DA")
        };
        Bitmap image = CreateNumberArrayImage(10, 10);
        
        var builder = new DiscordMessageBuilder()
            .WithEmbed(embed)
            .AddComponents(buttons);
        await ctx.RespondAsync(builder);
    }*/


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


