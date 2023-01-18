using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MainBot.Commands;
using Newtonsoft.Json;
using DSharpPlus.Menus;

namespace MainBot;


public class Bot : BackgroundService
{
    public DiscordClient Discord;
    public CommandsNextExtension Commands { get; set; }
    

    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        var json = "";
        using (var fs = File.OpenRead("config.json"))
        using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync();

       
        var cfgjson = JsonConvert.DeserializeObject<ConfigJson>(json);
        Dictionary<int, string> roles = new Dictionary<int, string>();
        roles = new Dictionary<int, string>()
        {
            [1] = "DEV",
            [2] = "admin",
            [3] = "help"
        };
        
               
                Discord = new DiscordClient(new DiscordConfiguration
                {
                    Token = cfgjson.Token,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.All
                });
                var ccfg = new CommandsNextConfiguration
                {
                    
                    StringPrefixes = new[] {cfgjson.CommandPrefix},

                    
                    EnableDms = true,
                    
                    EnableMentionPrefix = true
                };
                this.Commands = this.Discord.UseCommandsNext(ccfg);
                this.Commands.RegisterCommands<Moderating>();
                this.Commands.RegisterCommands<AdminCmds>();

                await Discord.ConnectAsync();
                await Task.Delay(-1);
                
                /*var options = new List<DiscordSelectComponentOption>()
                {
                    new DiscordSelectComponentOption(
                        "Label, no description",
                        "label_no_desc"),

                    new DiscordSelectComponentOption(
                        "Label, Description",
                        "label_with_desc",
                        "This is a description!"),

                    new DiscordSelectComponentOption(
                        "Label, Description, Emoji",
                        "label_with_desc_emoji",
                        "This is a description!",
                        emoji: new DiscordComponentEmoji(854260064906117121)),

                    new DiscordSelectComponentOption(
                        "Label, Description, Emoji (Default)",
                        "label_with_desc_emoji_default",
                        "This is a description!",
                        isDefault: true,
                        new DiscordComponentEmoji(854260064906117121))
                };

// Make the dropdown
                var dropdown = new DiscordSelectComponent("dropdown", null, options, false, 1, 2);
                DiscordChannel channel;
                Discord.MessageCreated += async (s, e) =>
                {
                    channel = e.Message.Channel.Id();
                    if (e.Message.Content.ToLower().StartsWith("menu"))
                    {
                        var builder = new DiscordMessageBuilder()
                            .WithContent("Look, it's a dropdown!")
                            .AddComponents(dropdown);
                        
                        await builder.SendAsync(channel);
                    }
                    
                    

                };*/
    }
}

public struct ConfigJson
{
    [JsonProperty("token")]
    public string Token { get; private set; }

    [JsonProperty("prefix")]
    public string CommandPrefix { get; private set; }
}