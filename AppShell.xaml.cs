namespace FoodSnap;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(Views.HomePage), typeof(Views.HomePage));
        Routing.RegisterRoute(nameof(Views.ScanPage), typeof(Views.ScanPage));
        Routing.RegisterRoute(nameof(Views.LocationPage), typeof(Views.LocationPage));
    }
}