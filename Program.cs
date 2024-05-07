using EvilBot.src;
using DotNetEnv;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus.VoiceNext;

namespace EvilBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Load environment variables from the .env file in the root directory
            Env.Load();

            // Setup the Serilog logger
            Logs.ConfigureLogger();

            // Log that a new instance of the bot has been launched
            Serilog.Log.Debug("New EvilBunny instance has been launched");

            // Get the Discord token from the environment variables
            string? discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                // If the token is not found, log an error and exit the program
                Serilog.Log.Fatal("Error: No discord token found. Please provide a token via the DISCORD_TOKEN environment variable.");
                Environment.Exit(1);
            }

            // Create a new Discord client with the provided token and configuration
            DiscordShardedClient discordClient = new(new DiscordConfiguration()
            {
                Token = discordToken,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMessages,
                LoggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory()
            });

            // Initialize VoiceNext for each shard
            foreach (var shard in discordClient.ShardClients.Values)
            {
                var vnext = shard.UseVoiceNext(new VoiceNextConfiguration
                {
                    AudioFormat = AudioFormat.Default,
                    EnableIncoming = false
                });
            }

            // Create a new service collection and add your services
            var services = new ServiceCollection()
                // Add other services as needed
                .BuildServiceProvider();

            // Use the commands extension
            IReadOnlyDictionary<int, CommandsExtension> commandsExtensions = await discordClient.UseCommandsAsync(new CommandsConfiguration()
            {
                ServiceProvider = services,
                RegisterDefaultCommandProcessors = true
            });

            // Iterate through each Discord shard
            foreach (CommandsExtension commandsExtension in commandsExtensions.Values)
            {
                // Add all commands by scanning the current assembly
                commandsExtension.AddCommands(typeof(Program).Assembly);
                TextCommandProcessor textCommandProcessor = new(new()
                {
                    PrefixResolver = new DefaultPrefixResolver("?").ResolvePrefixAsync
                });

                // Add text commands with a custom prefix (?ping)
                await commandsExtension.AddProcessorsAsync(textCommandProcessor);
            }

            // Start the Discord client
            await discordClient.StartAsync();

            // Keep the program running indefinitely
            await Task.Delay(-1);
        }
    }
}