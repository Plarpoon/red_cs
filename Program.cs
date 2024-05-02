using DSharpPlus;
using DotNetEnv;

namespace EvilBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //  Load env variables from the .env file in the root directory
            Env.Load();

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Env.GetString("DISCORD_TOKEN"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            discord.MessageCreated += async (s, e) =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("pong!");
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}