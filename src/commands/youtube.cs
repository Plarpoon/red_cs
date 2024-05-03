using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using System.Diagnostics;

namespace EvilBot.src.commands
{
    public class YoutubeCommand
    {
        [Command("join")]
        public static async Task JoinCommand(CommandContext ctx, DiscordChannel? channel = null)
        {
            if (ctx.Member?.VoiceState != null)
            {
                channel ??= ctx.Member.VoiceState.Channel;
            }

            if (channel is not null)
            {
                var connection = await channel.ConnectAsync();
                if (connection != null)
                {
                    _ = connection;
                }
            }
        }

        [Command("play")]
        public static async Task PlayCommand(CommandContext ctx, string path)
        {
            var vnext = ctx.Client.GetVoiceNext();

            if (ctx.Guild is not null)
            {
                var connection = vnext?.GetConnection(ctx.Guild);

                if (connection == null)
                {
                    await ctx.RespondAsync("Not connected to a voice channel.");
                    return;
                }

                var transmit = connection.GetTransmitSink();

                var pcm = ConvertAudioToPcm(path);
                await pcm.CopyToAsync(transmit);
                await pcm.DisposeAsync();
            }
        }

        [Command("leave")]
        public static async Task LeaveCommand(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();

            if (ctx.Guild is not null)
            {
                var connection = vnext?.GetConnection(ctx.Guild);

                if (connection == null)
                {
                    await ctx.RespondAsync("Not connected to a voice channel.");
                    return;
                }

                connection.Disconnect();
            }
        }

        private static Stream ConvertAudioToPcm(string filePath)
        {
            var ffmpeg = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $@"-i ""{filePath}"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            });

            return ffmpeg?.StandardOutput.BaseStream ?? Stream.Null;
        }
    }
}