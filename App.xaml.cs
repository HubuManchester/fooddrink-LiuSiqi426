using FoodSnap.Services;

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
        double smallFontSize = 10 * multiplier;
        double normalFontSize = baseFontSize * multiplier;
        double mediumFontSize = (baseFontSize + 2) * multiplier;
        double largeFontSize = (baseFontSize + 4) * multiplier;
        double titleFontSize = (baseFontSize + 8) * multiplier;
        double buttonFontSize = (baseFontSize + 2) * multiplier;
        double buttonHeight = Settings.IsAccessibilityEnabled ? 70 : 50;

        // Update app resources
        UpdateResource("SmallFontSize", smallFontSize);
        UpdateResource("NormalFontSize", normalFontSize);
        UpdateResource("MediumFontSize", mediumFontSize);
        UpdateResource("LargeFontSize", largeFontSize);
        UpdateResource("TitleFontSize", titleFontSize);
        UpdateResource("ButtonFontSize", buttonFontSize);
        UpdateResource("ButtonHeight", buttonHeight);
    }

    private static void UpdateResource(string key, object value)
    {
        if (Current.Resources.ContainsKey(key))
        {
            Current.Resources[key] = value;
        }
        else
        {
            Current.Resources.Add(key, value);
        }
    }
}