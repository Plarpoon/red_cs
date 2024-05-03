using EvilBot.src;
using DotNetEnv;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using Microsoft.Extensions.DependencyInjection;

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

            string? discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                Console.WriteLine("Error: No discord token found. Please provide a token via the DISCORD_TOKEN environment variable.");
                Environment.Exit(1);
            }

            DiscordShardedClient discordClient = new(new DiscordConfiguration()
            {
                Token = discordToken,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMessages,
                LoggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory()
            });

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

            await discordClient.StartAsync();
            await Task.Delay(-1);
        }
    }
}