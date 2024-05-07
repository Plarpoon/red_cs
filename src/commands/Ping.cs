using DSharpPlus.Commands;
using DSharpPlus.Entities;

// Define the namespace for the commands
namespace EvilBot.src.commands
{
    // Define the class for the Ping command
    public class PingCommand
    {
        // Define the command for ping
        [Command("ping")]
        public static async Task Execute(CommandContext ctx)
        {
            // Create a new embed message
            var embed = new DiscordEmbedBuilder
            {
                // Set the title of the embed
                Title = "🏓 Pong!",

                // Set the description of the embed, showing the WebSocket ping
                Description = $"WebSocket ping is {ctx.Client.Ping}ms",

                // Set the color of the embed to green
                Color = DiscordColor.Green,

                // Set the footer of the embed
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    // Set the text of the footer
                    Text = "Ping is checked once every minute to avoid rate limiting"
                }
            };

            // Send the embed message
            await ctx.RespondAsync(embed: embed);
        }
    }
}