using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using System.Diagnostics;

namespace FoodSnap;

public class LocationService
{
    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (permissionStatus != PermissionStatus.Granted)
            {
                await ShowPermissionDeniedAlert();
                return null;
            }

            var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(15)
            });

            return location;
        }
        catch (FeatureNotEnabledException)
        {
            await ShowLocationDisabledAlert();
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Location error: {ex.Message}");
            return null;
        }
    }

    private async Task ShowLocationDisabledAlert()
    {
        try
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Location Disabled",
                    "Please enable location services (GPS) in your device settings.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ShowLocationDisabledAlert error: {ex.Message}");
        }
    }

    private async Task ShowPermissionDeniedAlert()
    {
        try
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Permission Required",
                    "Location permission is needed to find nearby restaurants.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ShowPermissionDeniedAlert error: {ex.Message}");
        }
    }

    public async Task<List<Placemark>> GetNearbyPlacesAsync(Location location)
    {
        var places = new List<Placemark>();

        try
        {
            await Task.Delay(500);

            places.Add(new Placemark { FeatureName = "Healthy Bites Cafe", Thoroughfare = "123 Main Street", Locality = "Downtown" });
            places.Add(new Placemark { FeatureName = "Fresh Harvest Restaurant", Thoroughfare = "456 Oak Avenue", Locality = "Downtown" });
            places.Add(new Placemark { FeatureName = "Organic Corner", Thoroughfare = "789 Pine Street", Locality = "Downtown" });
            places.Add(new Placemark { FeatureName = "Green Leaf Bistro", Thoroughfare = "321 Maple Drive", Locality = "Downtown" });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GetNearbyPlacesAsync error: {ex.Message}");
        }

        return places;
    }
}