using Newtonsoft.Json;
using Raccoon.Ninja.Extensions.Desktop.Logging;
using Raccoon.Ninja.WForm.GlucoseIcon.Enums;
using Raccoon.Ninja.WForm.GlucoseIcon.Handlers.DataFetchers;
using Raccoon.Ninja.WForm.GlucoseIcon.Interfaces;
using Raccoon.Ninja.WForm.GlucoseIcon.Models;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Utils;

public static class AppSettings
{
    public static AppSettingsModel Config { get; private set; }
    
    private static bool _isWin11;
    private const string AppSettingsFile = "appsettings.json";
    private static readonly Version Win10Version = new (10, 0); // Windows 10 version
    private static readonly Version Win11Version = new (10, 0, 22000); // Windows 11 version

    public static void LoadSettings()
    {
        Logger.LogTrace("Loading application settings...");
        if (!File.Exists(AppSettingsFile))
            throw new FileNotFoundException($"The file {AppSettingsFile} was not found.");

        var json = File.ReadAllText(AppSettingsFile);
        Config = JsonConvert.DeserializeObject<AppSettingsModel>(json);

        var osVersion = Environment.OSVersion.Version;
        _isWin11 = osVersion >= Win11Version;
        
        Logger.LogInfo(_isWin11 ? "Detected App running on Windows 11" : "Detected App running on Windows 10");
        
        if (osVersion < Win10Version)
            Logger.LogInfo("This application was only tested on Windows 10 and 11");
        
        if (Config is not null)
            return;
        
        throw new ArgumentException($"The file {AppSettingsFile} could not be deserialized.");
    }

    public static IDataFetcher GetDataFetcherBasedOnSettings()
    {
        var logDataSourceType = Config.DataSource.SelectedSource.ToString(); 
        Logger.LogTrace("Getting data fetcher based on settings. Type: {DataSourceType}", logDataSourceType);
        return Config.DataSource.SelectedSource switch
        {
            DataSourceType.MongoDb => CreateFetcherForMongoDb(),
            DataSourceType.AzureFunction => CreateFetcherForAzureFunction(),
            _ => throw new ArgumentException($"The data source {Config.DataSource.SelectedSource} is not supported.")
        };
    }

    public static TaskbarIconOverlayFontConfig GetTaskbarIconOverlayBasedOnSettings()
    {
        return _isWin11 ? Config.Win11TaskbarIconOverlayFontConfig : Config.Win10TaskbarIconOverlayFontConfig;
    }
    
    public static NotificationIconFontConfig GetNotificationIconBasedOnSettings()
    {
        return _isWin11 ? Config.Win11NotificationIconFontConfig : Config.Win10NotificationIconFontConfig;
    }
    
    private static IDataFetcher CreateFetcherForMongoDb()
    {
        var config = Config.DataSource.MongoDb;
        return new MongoDbDataFetcher(config.ConnectionString, config.DatabaseName, config.CollectionName);
    }

    private static IDataFetcher CreateFetcherForAzureFunction()
    {
        var config = Config.DataSource.AzureFunction;
        return new AzureFunctionDataFetcher(config.BaseUrl, config.ApiKey, config.PostBodyText);
    }
}