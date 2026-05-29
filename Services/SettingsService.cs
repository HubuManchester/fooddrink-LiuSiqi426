using System.Text.Json;

namespace FoodSnap.Services;

public class SettingsService
{
    private const string SettingsFileName = "user_settings.json";
    private string _settingsPath;
    private UserSettings _settings;

    public SettingsService()
    {
        _settingsPath = Path.Combine(FileSystem.AppDataDirectory, SettingsFileName);
        _settings = new UserSettings();
        LoadSettings();
    }

    public bool IsDarkModeEnabled
    {
        get => _settings.IsDarkModeEnabled;
        set
        {
            if (_settings.IsDarkModeEnabled != value)
            {
                _settings.IsDarkModeEnabled = value;
                SaveSettings();
            }
        }
    }

    public string FontSize
    {
        get => _settings.FontSize;
        set
        {
            if (_settings.FontSize != value)
            {
                _settings.FontSize = value;
                SaveSettings();
            }
        }
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                string json = File.ReadAllText(_settingsPath);
                UserSettings? loaded = JsonSerializer.Deserialize<UserSettings>(json);
                if (loaded != null) _settings = loaded;
            }
        }
        catch { }
    }

    private void SaveSettings()
    {
        try
        {
            string json = JsonSerializer.Serialize(_settings);
            File.WriteAllText(_settingsPath, json);
        }
        catch { }
    }

    private class UserSettings
    {
        public bool IsDarkModeEnabled { get; set; } = false;
        public string FontSize { get; set; } = "Normal";
    }
}