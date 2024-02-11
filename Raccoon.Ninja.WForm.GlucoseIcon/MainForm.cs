using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Raccoon.Ninja.Extensions.Desktop.Logging;
using Raccoon.Ninja.WForm.GlucoseIcon.ExtensionMethods;
using Raccoon.Ninja.WForm.GlucoseIcon.Handlers;
using Raccoon.Ninja.WForm.GlucoseIcon.Interfaces;
using Raccoon.Ninja.WForm.GlucoseIcon.Models;
using Raccoon.Ninja.WForm.GlucoseIcon.Utils;

namespace Raccoon.Ninja.WForm.GlucoseIcon;

[ExcludeFromCodeCoverage]
public partial class MainForm : Form
{
    private TimerHandler _timerHandler;
    private NotifyIcon _notifyIcon;
    private IDataFetcher _dataFetcher;

    // Import the DestroyIcon extern method
    [LibraryImport("user32.dll")]
    private static partial int DestroyIcon(IntPtr handle);

// Wrapper method to call the P/Invoke method and convert the return value to bool
    private static void DestroyIconWrapper(IntPtr handle) {
        if (DestroyIcon(handle) == 0) return;

        Logger.LogTrace("DestroyIcon failed with error code: {ErrorCode}", Marshal.GetLastWin32Error());
    }
    
    public MainForm()
    {
        AppSettings.LoadSettings();
        InitializeComponent();
        InitializeNotifyIcon();
        InitializeHandlers();
    }

    private void InitializeNotifyIcon()
    {
        Logger.LogTrace("Initializing NotifyIcon");
        _notifyIcon = new NotifyIcon
        {
            Visible = true
        };
    }

    private void InitializeHandlers()
    {
        Logger.LogTrace("Initializing Handlers");
        _dataFetcher = AppSettings.GetDataFetcherBasedOnSettings();

        _timerHandler = new TimerHandler();
        var timerId = _timerHandler.AddTimer(AppSettings.Config.General.RefreshIntervalInMinutes);
        _timerHandler.AddAsyncTicker(timerId, _dataFetcher.FetchDataAsync);
        
        Logger.LogTrace("Subscribing to data fetcher events: Change Taskbar Icon");
        _dataFetcher.OnDataFetched += SetTaskbarIconOverlay;
        
        Logger.LogTrace("Subscribing to data fetcher events: Notification Icon");
        _dataFetcher.OnDataFetched += SetNotificationIcon;
        
        Logger.LogTrace("Starting data fetch loop");
        _timerHandler.StartTimer(timerId);
    }

    private static void DrawOnIcon(Graphics icon, float glucoseValue, string trendText, int firstLineSize, 
        int secondLineSize, float secondLineYPosition)
    {
        Logger.LogTrace("Drawing on icon. Glucose Value: {GlucoseValue}, Trend: {TrendText}", glucoseValue, trendText);
        
        // Set options for better quality
        icon.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

        // Draw first line
        icon.DrawString(
            glucoseValue.ToString(CultureInfo.InvariantCulture),
            new Font(AppSettings.Config.General.FontFamily, firstLineSize, FontStyle.Bold),
            Brushes.White,
            new PointF(0, 0)
        );
        
        // Draw second line
        icon.DrawString(
            trendText, 
            new Font(AppSettings.Config.General.FontFamily, secondLineSize), 
            Brushes.White, 
            new PointF(0, secondLineYPosition));
    }

    private void SetTaskbarIconOverlay(DataFetchResult dataFetched)
    {
        if (!dataFetched.Success)
        {
            Logger.LogTrace("Skipping Taskbar Icon Overlay update due to error: {Error}", dataFetched.ErrorMessage);
            return;
        }

        Logger.LogTrace("Updating Taskbar Icon Overlay");
        var secondLineText = dataFetched.Trend.ToTaskbarIconText();
        using var taskbarIconDefaultSize = new Bitmap(32, 32);
        var taskbarConfig = AppSettings.GetTaskbarIconOverlayBasedOnSettings();
        
        Logger.LogTrace("Creating a graphics from image");
        using var graphicsTaskbar = Graphics.FromImage(taskbarIconDefaultSize);
        
        DrawOnIcon(
            graphicsTaskbar, 
            dataFetched.GlucoseValue, 
            secondLineText,
            taskbarConfig.FirstLineFontSize,
            taskbarConfig.GetSecondLineFontSize(secondLineText),
            16);

        Logger.LogTrace( "Creating icon from bitmap");
        var taskbarIconHandle = taskbarIconDefaultSize.GetHicon();
        using var createdIcon = Icon.FromHandle(taskbarIconHandle);

        Logger.LogTrace( "Changing taskbar icon");
        Icon = (Icon)createdIcon.Clone();

        Logger.LogTrace("Releasing icon handle");
        DestroyIconWrapper(taskbarIconHandle);
    }

    private void SetNotificationIcon(DataFetchResult dataFetched)
    {
        if (!dataFetched.Success)
        {
            Logger.LogTrace("Skipping Notification Icon update due to error: {Error}", dataFetched.ErrorMessage);
            return;
        }

        Logger.LogTrace("Updating Notification Icon");
        var secondLineText = dataFetched.Trend.ToNotifyIconText();
        using var notificationIconDefaultSize = new Bitmap(16, 16);
        var notificationConfig = AppSettings.GetNotificationIconBasedOnSettings();
        
        Logger.LogTrace("Creating a graphics from image");
        using var graphicsNotification = Graphics.FromImage(notificationIconDefaultSize);
        
        DrawOnIcon(
            graphicsNotification, 
            dataFetched.GlucoseValue, 
            secondLineText,
            notificationConfig.FirstLineFontSize,
            notificationConfig.SecondLineFontSize,
            8);

        Logger.LogTrace("Creating icon from bitmap");
        var notificationIconHandle = notificationIconDefaultSize.GetHicon();
        using var createdIcon = Icon.FromHandle(notificationIconHandle);
        
        Logger.LogTrace("Changing notification icon");
        _notifyIcon.Icon = (Icon)createdIcon.Clone();

        Logger.LogTrace("Releasing icon handle");
        DestroyIconWrapper(notificationIconHandle);
    }
}
