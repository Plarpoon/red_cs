using System.Reflection;
using DotNetEnv;
using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace EvilBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Load env variables from the .env file in the root directory
            Env.Load();

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Env.GetString("DISCORD_TOKEN"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            // Add the SlashCommandsExtension to the DiscordClient
            var commands = discord.UseSlashCommands();

            // Get all types in the "EvilBot.src.commands" namespace that inherit from ApplicationCommandModule
            var commandModules = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == "EvilBot.src.commands" && t.IsSubclassOf(typeof(ApplicationCommandModule)));

            // Register all command modules
            foreach (var commandModule in commandModules)
            {
                commands.RegisterCommands(commandModule);
            }

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}