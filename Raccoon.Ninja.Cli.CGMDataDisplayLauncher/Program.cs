using System.Diagnostics;
using Raccoon.Ninja.Extensions.Desktop.Logging;

Logger.LogTrace("Starting Launcher");

//Keeping this as a constant, since it is the only thing that will change, to avoid magic strings and because I'm running
//another application and I want to avoid any obvious problems.
const string appName = "CGMDataDisplay.exe";

try
{
   Logger.LogTrace("Starting CGM Data Display application");
   Process.Start(new ProcessStartInfo
   {
      FileName = appName,
      WindowStyle = ProcessWindowStyle.Normal
   });
}
catch (Exception e)
{
   Logger.LogError("An unexpected error occurred while starting the main application ({AppName}): {Error}", e, appName, e.Message);
   throw;
}
finally
{
   Logger.LogTrace("Exiting Launcher");
}
