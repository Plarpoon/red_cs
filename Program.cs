using EvilBot.src;
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

            // Setup Serilog logger
            Logs.ConfigureLogger();

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Env.GetString("DISCORD_TOKEN"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
                LoggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(),
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

            // Subscribe to the Ready event
            discord.Ready += (s, e) =>
            {
                // Log when the bot is ready
                Serilog.Log.Information("Bot is ready");
                return Task.CompletedTask;
            };

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}