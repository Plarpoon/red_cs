using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace EvilBot.src.commands
{
    public class PingCommand
    {
        public static async Task Execute(DiscordClient client, MessageCreateEventArgs e)
        {
            if (e.Message.Content.StartsWith("ping", StringComparison.CurrentCultureIgnoreCase))
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "🏓 Pong!",
                    Description = $"WebSocket ping is {client.Ping}ms",
                    Color = DiscordColor.Green,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "Ping is checked once every minute to avoid rate limiting"
                    }
                };

                await e.Message.RespondAsync(embed: embed);
            }
        }
    }
}