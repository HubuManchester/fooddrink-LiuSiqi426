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

        // Apply font size
        double fontSize = Settings.FontSize switch
        {
            "Small" => 12.0,
            "Large" => 18.0,
            "ExtraLarge" => 22.0,
            _ => 14.0
        };

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
            Current.Resources["ButtonFontSize"] = fontSize + 2;
        }
        else
        {
            Current.Resources.Add("ButtonFontSize", fontSize + 2);
        }

        if (Current.Resources.ContainsKey("TitleFontSize"))
        {
            Current.Resources["TitleFontSize"] = fontSize + 8;
        }
        else
        {
            Current.Resources.Add("TitleFontSize", fontSize + 8);
        }
    }
}