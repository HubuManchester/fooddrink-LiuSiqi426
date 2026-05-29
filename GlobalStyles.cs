using Microsoft.Maui.Controls;

namespace FoodSnap.Styles
{
    public static class GlobalStyles
    {
        private static double _currentFontSize = 14;

        public static double CurrentFontSize
        {
            get => _currentFontSize;
            set
            {
                _currentFontSize = value;
                ApplyFontSize();
            }
        }

        public static void ApplyFontSize()
        {
            var app = Application.Current;
            if (app == null) return;

            // Update font size in app resources
            app.Resources["NormalFontSize"] = _currentFontSize;
            app.Resources["LargeFontSize"] = _currentFontSize + 4;
            app.Resources["TitleFontSize"] = _currentFontSize + 8;
            app.Resources["ButtonFontSize"] = _currentFontSize + 2;
        }
    }
}