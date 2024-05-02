using DSharpPlus.EventArgs;

namespace EvilBot.src.commands
{
    public class PingCommand
    {
        public static async Task Execute(MessageCreateEventArgs e)
        {
            if (e.Message.Content.StartsWith("ping", StringComparison.CurrentCultureIgnoreCase))
                await e.Message.RespondAsync("pong!");
        }
    }
}