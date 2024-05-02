using DSharpPlus;
using DotNetEnv;
using System.Reflection;
using DSharpPlus.EventArgs;
using DSharpPlus.AsyncEvents;

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

            // Load all commands using reflection
            var commandTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                // Filter the types to only those in the "EvilBot.src.commands" namespace and have a method named "Execute"
                .Where(t => t.Namespace == "EvilBot.src.commands" && t.GetMethods().Any(m => m.Name == "Execute"));

            foreach (var commandType in commandTypes)
            {
                // Get the "Execute" method from the command type
                var executeMethod = commandType.GetMethod("Execute");
                // Check if the "Execute" method exists
                if (executeMethod != null)
                {
                    // Create an instance of the command type if the "Execute" method is not static
                    // If the "Execute" method is static, instance remains null
                    object? instance = executeMethod.IsStatic ? null : Activator.CreateInstance(commandType);
                    if (instance != null || executeMethod.IsStatic)
                    {
                        // Add a new event handler to the MessageCreated event of the discord client
                        // The event handler invokes the "Execute" method of the command type
                        discord.MessageCreated += new AsyncEventHandler<DiscordClient, MessageCreateEventArgs>((client, e) => (Task?)executeMethod.Invoke(instance, new object[] { client, e }));
                    }
                }
            }

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}