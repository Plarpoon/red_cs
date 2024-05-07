using Serilog;
using Serilog.Events;

namespace EvilBot.src
{
    public static class Logs
    {
        // This method is used to configure the logger
        public static void ConfigureLogger()
        {
            // Create a new logger configuration
            Log.Logger = new LoggerConfiguration()
                // Ignore all log events from Microsoft that are not Fatal
                .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                // Set the minimum log level to Debug
                .MinimumLevel.Debug()
                // Write the log to the console
                .WriteTo.Console()
                // Write the log to a file
                .WriteTo.File($"logs/log-{DateTime.Now:dd-MM-yyyy}.txt",
                    // Define the output template for the log
                    outputTemplate: "{Timestamp:HH:mm} [{Level}] {Message}{NewLine}{Exception}",
                    // Roll over to a new file when the file size limit is reached
                    rollOnFileSizeLimit: true,
                    // Retain a maximum of 5 log files
                    retainedFileCountLimit: 5)
                // Create the logger with the defined configuration
                .CreateLogger();

            // Log an information message indicating that Serilog has been initialized
            Log.Information("Serilog has been initialized");
        }
    }
}