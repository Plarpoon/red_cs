using DSharpPlus.Commands;
using DSharpPlus.VoiceNext;

// Define the namespace for the commands
namespace EvilBot.src.commands
{
    // Define the class for the Youtube command
    public class YoutubeCommand
    {
        // Define the command for listing channels
        [Command("listvc")]
        public static async Task ListChannels(CommandContext ctx)
        {
            // Get the guild (server) where the command was issued
            var guild = ctx.Guild;

            // Get a list of voice channels in the guild
            var voiceChannels = guild?.Channels.Values.Where(c => c.Type == DSharpPlus.Entities.DiscordChannelType.Voice).ToList();

            // Prepare the response message
            string response = "Available voice channels:\n";

            // Add each channel to the response message
            if (voiceChannels is not null)
            {
                foreach (var channel in voiceChannels)
                {
                    response += $"- {channel.Name}\n";
                }
            }

            // Send the response message
            await ctx.RespondAsync(response);
        }

        // Define the command for joining a channel
        [Command("joinvc")]
        public static async Task JoinChannel(CommandContext ctx, string channelName)
        {
            // Get the guild (server) where the command was issued
            var guild = ctx.Guild;

            // Find the channel with the given name
            var channel = guild?.Channels.Values.FirstOrDefault(c => c.Type == DSharpPlus.Entities.DiscordChannelType.Voice && c.Name == channelName);

            // If the channel was found...
            if (channel is not null)
            {
                // Connect to the channel
                await channel.ConnectAsync();

                // Send a message indicating the bot has joined the channel
                await ctx.RespondAsync($"Joined channel: {channelName}");
            }
            else
            {
                // If the channel was not found, send a message indicating this
                await ctx.RespondAsync($"No voice channel found with the name: {channelName}");
            }
        }
    }
}