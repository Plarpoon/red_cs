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
            channel ??= ctx.Member.VoiceState?.Channel;
            _ = await channel.ConnectAsync();
        }

        [Command("play")]
        public async Task PlayCommand(CommandContext ctx, string path)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);

            var transmit = connection.GetTransmitSink();

            var pcm = ConvertAudioToPcm(path);
            await pcm.CopyToAsync(transmit);
            await pcm.DisposeAsync();
        }

        [Command("leave")]
        public static void LeaveCommand(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);

            connection.Disconnect();
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

            return ffmpeg.StandardOutput.BaseStream;
        }
    }
}