using Raccoon.Ninja.Domain.Core.Models;
using Raccoon.Ninja.WForm.GlucoseIcon.Enums;

namespace Raccoon.Ninja.WForm.GlucoseIcon.Models;

public record AppSettingsModel
{
    // For Settings tab - General
    public GeneralSettings General { get; init; }
    
    // For Settings tab - Data Source Selection
    public DataSource DataSource { get; init; }
    
    // For Settings tab - Taskbar Icon Overlay
    public TaskbarIconOverlayFontConfig Win10TaskbarIconOverlayFontConfig { get; init; }
    public TaskbarIconOverlayFontConfig Win11TaskbarIconOverlayFontConfig { get; init; }

    // For Settings tab - Notification Icon
    public NotificationIconFontConfig Win10NotificationIconFontConfig { get; init; }
    public NotificationIconFontConfig Win11NotificationIconFontConfig { get; init; }

}

public record GeneralSettings
{
    public int RefreshIntervalInMinutes { get; init; }
    public string FontFamily { get; init; }
}

public record DataSource
{
    public DataSourceType SelectedSource { get; init; }  //Enum: 1 = Mongo DB / 2 = Azure Function

    // For Azure Function
    public DataSourceAzureFunction AzureFunction { get; init; }

    // For MongoDB
    public DataSourceMongoDb MongoDb { get; init; }
}

public record DataSourceAzureFunction
{
    public string BaseUrl { get; init; }
    public string ApiKey { get; init; } 
    public GetDataRequest PostBodyText { get; init; }
       
}

public record DataSourceMongoDb
{
    public string ConnectionString { get; init; }
    public string DatabaseName { get; init; }
    public string CollectionName { get; init; }    
}

public record TaskbarIconOverlayFontConfig
{
    public int FirstLineFontSize { get; init; }
    public int SecondLineFontSize1Char { get; init; }
    public int SecondLineFontSize2Char { get; init; }
    public int SecondLineFontSize3Char { get; init; }
}

public record NotificationIconFontConfig
{
    public int FirstLineFontSize { get; init; }
    public int SecondLineFontSize { get; init; }
}
