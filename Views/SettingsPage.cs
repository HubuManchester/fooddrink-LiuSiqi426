using FoodSnap.Services;
using FoodSnap.Styles;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace FoodSnap.Views
{
    public class SettingsPage : ContentPage
    {
        private Button? lightBtn;
        private Button? darkBtn;
        private Button? smallBtn;
        private Button? normalBtn;
        private Button? largeBtn;
        private Label? themeLabel;
        private Label? previewLabel;
        private Switch? accessibilitySwitch;
        private Label? accessibilityStatusLabel;
        private Label? titleLabel;
        private Label? accessibilityTitleLabel;
        private Label? accessibilityDescLabel;
        private Label? switchLabel;
        private Label? fontSizeTitleLabel;

        public SettingsPage()
        {
            Title = "Settings";
            BuildUI();
            UpdateUI();
            ApplyThemeColors();
        }

        private void BuildUI()
        {
            var layout = new VerticalStackLayout { Spacing = 15, Padding = 15 };

            // Title
            titleLabel = new Label
            {
                Text = "Settings",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            layout.Children.Add(titleLabel);

            // Theme Grid
            var themeGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 10
            };

            lightBtn = new Button { Text = "☀️ Light", CornerRadius = 10 };
            lightBtn.Clicked += (s, e) => OnLightModeClicked();

            darkBtn = new Button { Text = "🌙 Dark", CornerRadius = 10 };
            darkBtn.Clicked += (s, e) => OnDarkModeClicked();

            themeGrid.Add(lightBtn, 0, 0);
            themeGrid.Add(darkBtn, 1, 0);
            layout.Children.Add(themeGrid);

            themeLabel = new Label
            {
                Text = "Current: Light Mode",
                FontSize = 12,
                HorizontalOptions = LayoutOptions.Center
            };
            layout.Children.Add(themeLabel);

            // Accessibility Mode Section
            var accessibilityFrame = new Frame
            {
                CornerRadius = 10,
                Padding = 10,
                Margin = new Thickness(0, 10, 0, 0),
                BackgroundColor = Colors.Transparent
            };

            var accessibilityLayout = new VerticalStackLayout { Spacing = 10 };

            accessibilityTitleLabel = new Label
            {
                Text = "♿ Accessibility Mode",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            };
            accessibilityLayout.Children.Add(accessibilityTitleLabel);

            accessibilityDescLabel = new Label
            {
                Text = "When enabled: Larger text, bigger buttons, high contrast",
                FontSize = 12
            };
            accessibilityLayout.Children.Add(accessibilityDescLabel);

            var switchRow = new HorizontalStackLayout { Spacing = 15 };
            accessibilitySwitch = new Switch();
            accessibilitySwitch.Toggled += OnAccessibilityToggled;
            switchRow.Add(accessibilitySwitch);

            switchLabel = new Label { Text = "Enable Accessibility Mode", VerticalOptions = LayoutOptions.Center };
            switchRow.Add(switchLabel);

            accessibilityLayout.Children.Add(switchRow);

            accessibilityStatusLabel = new Label
            {
                Text = "Status: Disabled",
                FontSize = 12
            };
            accessibilityLayout.Children.Add(accessibilityStatusLabel);

            accessibilityFrame.Content = accessibilityLayout;
            layout.Children.Add(accessibilityFrame);

            // Font Size Title
            fontSizeTitleLabel = new Label { Text = "Font Size", FontAttributes = FontAttributes.Bold };
            layout.Children.Add(fontSizeTitleLabel);

            // Font Size Grid
            var fontGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 10,
                Margin = new Thickness(0, 10, 0, 0)
            };

            smallBtn = new Button { Text = "A", FontSize = 12, CornerRadius = 10 };
            smallBtn.Clicked += (s, e) => OnSmallFontClicked();

            normalBtn = new Button { Text = "A", FontSize = 16, CornerRadius = 10 };
            normalBtn.Clicked += (s, e) => OnNormalFontClicked();

            largeBtn = new Button { Text = "A", FontSize = 20, CornerRadius = 10 };
            largeBtn.Clicked += (s, e) => OnLargeFontClicked();

            fontGrid.Add(smallBtn, 0, 0);
            fontGrid.Add(normalBtn, 1, 0);
            fontGrid.Add(largeBtn, 2, 0);
            layout.Children.Add(fontGrid);

            previewLabel = new Label
            {
                Text = "The quick brown fox jumps over the lazy dog.",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 15, 0, 0),
                LineBreakMode = LineBreakMode.WordWrap
            };
            layout.Children.Add(previewLabel);

            Content = new ScrollView { Content = layout };
        }

        private void ApplyThemeColors()
        {
            bool isDark = App.Settings.IsDarkModeEnabled;

            // Main text color for dark mode: bright cream, for light mode: dark gray
            Color mainTextColor = isDark ? Color.FromArgb("#F2EFE4") : Color.FromArgb("#555555");
            Color secondaryTextColor = isDark ? Color.FromArgb("#C0C0C0") : Color.FromArgb("#999999");

            // Apply to all labels
            if (titleLabel != null) titleLabel.TextColor = mainTextColor;
            if (themeLabel != null) themeLabel.TextColor = mainTextColor;
            if (accessibilityTitleLabel != null) accessibilityTitleLabel.TextColor = mainTextColor;
            if (accessibilityDescLabel != null) accessibilityDescLabel.TextColor = secondaryTextColor;
            if (switchLabel != null) switchLabel.TextColor = mainTextColor;
            if (accessibilityStatusLabel != null) accessibilityStatusLabel.TextColor = secondaryTextColor;
            if (fontSizeTitleLabel != null) fontSizeTitleLabel.TextColor = mainTextColor;
            if (previewLabel != null) previewLabel.TextColor = mainTextColor;
        }

        private void UpdateUI()
        {
            bool isDark = App.Settings.IsDarkModeEnabled;

            if (lightBtn != null)
                lightBtn.BackgroundColor = !isDark ? Color.FromArgb("#D4B0B5") : Color.FromArgb("#9E9E9E");
            if (darkBtn != null)
                darkBtn.BackgroundColor = isDark ? Color.FromArgb("#BC8E87") : Color.FromArgb("#9E9E9E");
            if (themeLabel != null)
                themeLabel.Text = isDark ? "Dark Mode" : "Light Mode";

            string font = App.Settings.FontSize;

            if (smallBtn != null)
                smallBtn.BackgroundColor = font == "Small" ? Color.FromArgb("#D4B0B5") : Color.FromArgb("#9E9E9E");
            if (normalBtn != null)
                normalBtn.BackgroundColor = font == "Normal" ? Color.FromArgb("#D4B0B5") : Color.FromArgb("#9E9E9E");
            if (largeBtn != null)
                largeBtn.BackgroundColor = font == "Large" ? Color.FromArgb("#D4B0B5") : Color.FromArgb("#9E9E9E");

            double size = font == "Small" ? 12 : (font == "Large" ? 18 : 14);
            if (previewLabel != null)
                previewLabel.FontSize = size;

            // Update accessibility UI
            if (accessibilitySwitch != null)
                accessibilitySwitch.IsToggled = App.Settings.IsAccessibilityEnabled;
            if (accessibilityStatusLabel != null)
                accessibilityStatusLabel.Text = App.Settings.IsAccessibilityEnabled ? "Status: Enabled (Large text, bigger buttons)" : "Status: Disabled";

            // Apply theme colors after UI update
            ApplyThemeColors();
        }

        private async void OnLightModeClicked()
        {
            App.Settings.IsDarkModeEnabled = false;
            App.ApplyUserSettings();
            UpdateUI();
            await DisplayAlert("Theme", "Light mode enabled. Restart app for full effect.", "OK");
        }

        private async void OnDarkModeClicked()
        {
            App.Settings.IsDarkModeEnabled = true;
            App.ApplyUserSettings();
            UpdateUI();
            await DisplayAlert("Theme", "Dark mode enabled. Restart app for full effect.", "OK");
        }

        private void OnSmallFontClicked()
        {
            App.Settings.FontSize = "Small";
            App.ApplyUserSettings();
            UpdateUI();
        }

        private void OnNormalFontClicked()
        {
            App.Settings.FontSize = "Normal";
            App.ApplyUserSettings();
            UpdateUI();
        }

        private void OnLargeFontClicked()
        {
            App.Settings.FontSize = "Large";
            App.ApplyUserSettings();
            UpdateUI();
        }

        private async void OnAccessibilityToggled(object sender, ToggledEventArgs e)
        {
            App.Settings.IsAccessibilityEnabled = e.Value;
            App.ApplyUserSettings();
            UpdateUI();

            string message = e.Value
                ? "Accessibility mode enabled.\n\n• Larger text\n• Bigger buttons\n• Better contrast\n\nRestart app for full effect."
                : "Accessibility mode disabled. Restart app for full effect.";

            await DisplayAlert("Accessibility Mode", message, "OK");
        }
    }
}