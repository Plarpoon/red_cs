using Serilog;
using Serilog.Events;

namespace EvilBot.src
{
    public static class Logs
    {
        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal) // Ignore non-fatal log events from Microsoft
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File($"logs/log-{DateTime.Now:dd-MM-yyyy}.txt",
                    outputTemplate: "{Timestamp:HH:mm} [{Level}] {Message}{NewLine}{Exception}",
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 5)
                .CreateLogger();
                
            Log.Information("Serilog has been initialized");
        }
    }
}