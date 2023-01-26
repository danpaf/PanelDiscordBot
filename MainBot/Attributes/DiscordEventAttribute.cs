namespace MainBot.Attributes;

public class DiscordEventAttribute : Attribute
{

    public readonly string MethodName;
    
    public DiscordEventAttribute(string methodName)
    {
        MethodName = methodName;
    }

}
