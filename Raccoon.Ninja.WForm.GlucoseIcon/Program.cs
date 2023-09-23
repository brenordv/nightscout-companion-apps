using Raccoon.Ninja.Extensions.Desktop.Logging;

namespace Raccoon.Ninja.WForm.GlucoseIcon;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        try
        {
            Logger.LogTrace("Starting application");
            Logger.LogTrace("Initializing application configuration");
            ApplicationConfiguration.Initialize();

            Logger.LogTrace("Starting Main form");
            Application.Run(new MainForm());
        }
        catch (Exception e)
        {
            Logger.LogError("An unexpected error occurred while running the application: {Error}", e, e.Message);
            throw;
        }
        finally
        {
            Logger.LogTrace("Exiting application");
        }
    }
}