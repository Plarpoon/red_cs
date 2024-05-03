/* using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EvilBot.src.commands
{
    public class YoutubeCommands : ApplicationCommandModule
    {
        public async Task Join(InteractionContext ctx, DiscordChannel? channel = null)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var vnc = vnext.GetConnection(ctx.Guild);

            if (vnc != null)
                throw new InvalidOperationException("Already connected in this guild.");

            if (channel == null)
            {
                if (ctx.Member?.VoiceState?.Channel == null)
                    throw new InvalidOperationException("You need to be in a voice channel.");
                else
                    channel = ctx.Member.VoiceState.Channel;
            }

            if (channel == null)
                throw new InvalidOperationException("Channel cannot be null.");

            vnc = await vnext.ConnectAsync(channel);
        }

        [SlashCommand("play", "Plays a YouTube video in the voice channel where the bot is connected")]
        public async Task Play(InteractionContext ctx, [Option("url", "YouTube URL to play")] string url)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var vnc = vnext.GetConnection(ctx.Guild);

            if (vnc == null)
                throw new InvalidOperationException("Not connected in a voice channel.");

            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            if (streamInfo != null)
            {
                var tempDirectory = "youtube_temp_dl";
                Directory.CreateDirectory(tempDirectory);

                var tempFiles = Directory.GetFiles(tempDirectory);
                foreach (var file in tempFiles)
                {
                    File.Delete(file);
                }

                var tempFilePath = Path.Combine(tempDirectory, $"{video.Id}.mp3");
                await youtube.Videos.Streams.DownloadAsync(streamInfo, tempFilePath);

                var pcm = ConvertAudioToPcm(tempFilePath);
                var transmit = vnc.GetTransmitSink();

                await pcm.CopyToAsync(transmit);
                await pcm.DisposeAsync();
            }
        }

        [SlashCommand("disconnect", "Disconnects the bot from the voice channel")]
        public async Task Disconnect(InteractionContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var vnc = vnext.GetConnection(ctx.Guild);

            if (vnc == null)
                throw new InvalidOperationException("Not connected in a voice channel.");

            vnc.Disconnect();
            await Task.CompletedTask; // To avoid CS1998 warning
        }
        private Stream ConvertAudioToPcm(string filePath)
        {
            var ffmpeg = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $@"-i ""{filePath}"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            });

            if (ffmpeg?.StandardOutput == null)
                throw new InvalidOperationException("ffmpeg.StandardOutput cannot be null.");

            return ffmpeg.StandardOutput.BaseStream;
        }
    }
} */