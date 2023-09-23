using System.Globalization;
using Raccoon.Ninja.WForm.GlucoseIcon.ExtensionMethods;
using Raccoon.Ninja.WForm.GlucoseIcon.Handlers;
using Raccoon.Ninja.WForm.GlucoseIcon.Interfaces;
using Raccoon.Ninja.WForm.GlucoseIcon.Models;
using Raccoon.Ninja.WForm.GlucoseIcon.Utils;

namespace Raccoon.Ninja.WForm.GlucoseIcon;

public partial class MainForm : Form
{
    private TimerHandler _timerHandler;
    private NotifyIcon _notifyIcon;
    private IDataFetcher _dataFetcher;


    // Import the DestroyIcon extern method
    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    private static extern bool DestroyIcon(IntPtr handle);

    public MainForm()
    {
        AppSettings.LoadSettings();
        InitializeComponent();
        InitializeNotifyIcon();
        InitializeHandlers();
    }

    private void InitializeNotifyIcon()
    {
        _notifyIcon = new NotifyIcon
        {
            Visible = true
        };
    }

    private void InitializeHandlers()
    {
        _dataFetcher = AppSettings.GetDataFetcherBasedOnSettings();
        _timerHandler = new TimerHandler();
        var timerId = _timerHandler.AddTimer(AppSettings.Config.General.RefreshIntervalInMinutes);
        _timerHandler.AddAsyncTicker(timerId, _dataFetcher.FetchDataAsync);
        _dataFetcher.OnDataFetched += SetTaskbarIconOverlay;
        _dataFetcher.OnDataFetched += SetNotificationIcon;
        
        _timerHandler.StartTimer(timerId);
    }

    private static void DrawOnIcon(Graphics icon, float glucoseValue, string trendText, int firstLineSize, 
        int secondLineSize, float secondLineYPosition)
    {
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
            return;

        var secondLineText = dataFetched.Trend.ToTaskbarIconText();
        using var taskbarIconDefaultSize = new Bitmap(32, 32);
        var taskbarConfig = AppSettings.GetTaskbarIconOverlayBasedOnSettings();
        using (var graphicsTaskbar = Graphics.FromImage(taskbarIconDefaultSize))
        {
            DrawOnIcon(
                graphicsTaskbar, 
                dataFetched.GlucoseValue, 
                secondLineText,
                taskbarConfig.FirstLineFontSize,
                taskbarConfig.GetSecondLineFontSize(secondLineText),
                16);
        }

        // Create an icon from the bitmap
        var taskbarIconHandle = taskbarIconDefaultSize.GetHicon();
        using (var createdIcon = Icon.FromHandle(taskbarIconHandle))
        {
            Icon = (Icon)createdIcon.Clone();
        }

        // Release the handle of the created icon
        DestroyIcon(taskbarIconHandle);
    }

    private void SetNotificationIcon(DataFetchResult dataFetched)
    {
        if (!dataFetched.Success)
            return;

        var secondLineText = dataFetched.Trend.ToNotifyIconText();
        using var notificationIconDefaultSize = new Bitmap(16, 16);
        var notificationConfig = AppSettings.GetNotificationIconBasedOnSettings();
        using (var graphicsNotification = Graphics.FromImage(notificationIconDefaultSize))
        {
            DrawOnIcon(
                graphicsNotification, 
                dataFetched.GlucoseValue, 
                secondLineText,
                notificationConfig.FirstLineFontSize,
                notificationConfig.SecondLineFontSize,
                8);
        }

        // Create an icon from the bitmap
        var notificationIconHandle = notificationIconDefaultSize.GetHicon();
        using (var createdIcon = Icon.FromHandle(notificationIconHandle))
        {
            // Set the icon to NotifyIcon
            _notifyIcon.Icon = (Icon)createdIcon.Clone();
        }

        // Release the handle of the created icon
        DestroyIcon(notificationIconHandle);
    }
}