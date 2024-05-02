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
                .Where(t => t.Namespace == "EvilBot.src.commands" && t.GetMethods().Any(m => m.Name == "Execute"));

            foreach (var commandType in commandTypes)
            {
                var executeMethod = commandType.GetMethod("Execute");
                if (executeMethod != null)
                {
                    object? instance = executeMethod.IsStatic ? null : Activator.CreateInstance(commandType);
                    if (instance != null || executeMethod.IsStatic)
                    {
                        discord.MessageCreated += new AsyncEventHandler<DiscordClient, MessageCreateEventArgs>((client, e) => (Task?)executeMethod.Invoke(instance, [e]));
                    }
                }
            }

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}