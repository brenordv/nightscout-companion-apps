using Raccoon.Ninja.WForm.GlucoseIcon.Models;

namespace Raccoon.Ninja.WForm.GlucoseIcon.ExtensionMethods;

public static class TaskbarIconOverlayExtensions
{
    public static int GetSecondLineFontSize(this TaskbarIconOverlayFontConfig taskbarIconOverlayFontConfig, string secondLineText)
    {
        return secondLineText.Length switch
        {
            >= 3 => taskbarIconOverlayFontConfig.SecondLineFontSize3Char,
            >= 2 => taskbarIconOverlayFontConfig.SecondLineFontSize2Char,
            _ => taskbarIconOverlayFontConfig.SecondLineFontSize1Char
        };
    }
}