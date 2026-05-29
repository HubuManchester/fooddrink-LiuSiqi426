using FoodSnap.Services;
using FoodSnap.Styles;

namespace FoodSnap;

public partial class App : Application
{
    private static SettingsService? _settingsService;

    public static SettingsService Settings
    {
        get
        {
            _settingsService ??= new SettingsService();
            return _settingsService;
        }
    }

    public App()
    {
        InitializeComponent();
        ApplyUserSettings();
        MainPage = new AppShell();
    }

    public static void ApplyUserSettings()
    {
        // Apply user selected theme
        if (Settings.IsDarkModeEnabled)
        {
            Current.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            Current.UserAppTheme = AppTheme.Light;
        }

        // Calculate base font size
        double baseFontSize = Settings.FontSize switch
        {
            "Small" => 12.0,
            "Large" => 18.0,
            "ExtraLarge" => 22.0,
            _ => 14.0
        };

        // Apply accessibility multiplier (1.5x if enabled)
        double multiplier = Settings.IsAccessibilityEnabled ? 1.5 : 1.0;
        double fontSize = baseFontSize * multiplier;
        double buttonFontSize = (baseFontSize + 2) * multiplier;
        double titleFontSize = (baseFontSize + 8) * multiplier;

        // Update app resources
        if (Current.Resources.ContainsKey("NormalFontSize"))
        {
            Current.Resources["NormalFontSize"] = fontSize;
        }
        else
        {
            Current.Resources.Add("NormalFontSize", fontSize);
        }

        if (Current.Resources.ContainsKey("ButtonFontSize"))
        {
            Current.Resources["ButtonFontSize"] = buttonFontSize;
        }
        else
        {
            Current.Resources.Add("ButtonFontSize", buttonFontSize);
        }

        if (Current.Resources.ContainsKey("TitleFontSize"))
        {
            Current.Resources["TitleFontSize"] = titleFontSize;
        }
        else
        {
            Current.Resources.Add("TitleFontSize", titleFontSize);
        }

        // Update button height for accessibility
        double buttonHeight = Settings.IsAccessibilityEnabled ? 70 : 50;
        if (Current.Resources.ContainsKey("ButtonHeight"))
        {
            Current.Resources["ButtonHeight"] = buttonHeight;
        }
        else
        {
            Current.Resources.Add("ButtonHeight", buttonHeight);
        }
    }
}