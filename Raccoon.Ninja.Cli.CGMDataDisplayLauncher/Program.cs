using System.Diagnostics;
using Raccoon.Ninja.Extensions.Desktop.Logging;

Logger.LogTrace("Starting Launcher");

//Keeping this as a constant, since it is the only thing that will change, to avoid magic strings and because I'm running
//another application and I want to avoid any obvious problems.
const string appName = "CGMDataDisplay.exe";

try
{
   Logger.LogTrace("Starting CGM Data Display application");

   // Construct the fully qualified path to the application
   var appFullPath = GetFullPathToApp(appName);

   Process.Start(new ProcessStartInfo
   {
      FileName = appFullPath,
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

return;

static string GetFullPathToApp(string appName)
{
   var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
   var assemblyPath = Path.GetDirectoryName(assemblyLocation);

   // Construct the fully qualified path to the application
   return string.IsNullOrWhiteSpace(assemblyPath) 
   ? appName
   : Path.Combine(assemblyPath, appName);
}
