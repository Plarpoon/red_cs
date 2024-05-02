using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;

namespace EvilBot.src.commands
{
    public class PingCommand : ApplicationCommandModule
    {
        [SlashCommand("ping", "Checks the bot's ping to the Discord API")]
        public static async Task Execute(InteractionContext ctx)
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

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }
    }
}