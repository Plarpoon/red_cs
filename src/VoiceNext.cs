using DSharpPlus;
using DSharpPlus.VoiceNext;

namespace EvilBot.src
{
    public static class VoiceNextConfig
    {
        public static void EnableVoiceNext(DiscordShardedClient discordClient)
        {
            // Enable the VoiceNext package for each shard.
            foreach (var shard in discordClient.ShardClients.Values)
            {
                shard.UseVoiceNext();
            }
        }
    }
}