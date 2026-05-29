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

        public SettingsPage()
        {
            Title = "Settings";
            BuildUI();
            UpdateUI();
        }

        private void BuildUI()
        {
            var layout = new VerticalStackLayout { Spacing = 15, Padding = 15 };

            layout.Children.Add(new Label
            {
                Text = "Settings",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            });

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

            layout.Children.Add(new Label { Text = "Font Size", FontAttributes = FontAttributes.Bold });
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
    }
}