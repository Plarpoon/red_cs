using DSharpPlus.Commands;
using DSharpPlus.Entities;

namespace EvilBot.src.commands
{
    public class PingCommand
    {
        [Command("ping")]
        public async Task Execute(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "🏓 Pong!",
                Description = $"WebSocket ping is {ctx.Client.Ping}ms",
                Color = DiscordColor.Green,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "Ping is checked once every minute to avoid rate limiting"
                }
            };

            await ctx.RespondAsync(embed: embed);
        }
    }
}