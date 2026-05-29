using Microsoft.Maui.Devices.Sensors;
using System.Diagnostics;

namespace FoodSnap.Views;

public partial class LocationPage : ContentPage
{
    private Location? _currentLocation;
    private LocationService? _locationService;
    private List<Placemark>? _nearbyPlaces;
    private CancellationTokenSource? _ttsCancellationTokenSource;

    public LocationPage()
    {
        try
        {
            InitializeComponent();
            _locationService = new LocationService();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"LocationPage init error: {ex.Message}");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (locationStatusLabel != null)
            {
                locationStatusLabel.Text = "Tap 📍 button to get location";
                locationStatusLabel.TextColor = Colors.Gray;
            }
            if (placesListLabel != null)
            {
                placesListLabel.Text = "Get location first, then tap 🍽️";
                placesListLabel.TextColor = Colors.Gray;
            }
            if (findPlacesButton != null) findPlacesButton.IsEnabled = false;
            if (speakLocationButton != null) speakLocationButton.IsEnabled = false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"LocationPage OnAppearing error: {ex.Message}");
        }
    }

    private async void OnGetLocationClicked(object sender, EventArgs e)
    {
        try
        {
            if (getLocationButton != null) getLocationButton.IsEnabled = false;
            if (locationStatusLabel != null)
            {
                locationStatusLabel.Text = "Getting location...";
                locationStatusLabel.TextColor = Colors.DarkOrange;
            }

            if (_locationService == null)
                _locationService = new LocationService();

            _currentLocation = await _locationService.GetCurrentLocationAsync();

            if (_currentLocation != null)
            {
                string locationText = $"Lat: {_currentLocation.Latitude:F4}, Lon: {_currentLocation.Longitude:F4}";
                if (locationStatusLabel != null)
                {
                    locationStatusLabel.Text = locationText;
                    locationStatusLabel.TextColor = Colors.Green;
                }
                if (findPlacesButton != null) findPlacesButton.IsEnabled = true;
                if (speakLocationButton != null) speakLocationButton.IsEnabled = true;

                await DisplayAlert("Location Found",
                    $"Your location:\n{locationText}\n\nTap 🍽️ to find nearby restaurants.", "OK");
            }
            else
            {
                if (locationStatusLabel != null)
                {
                    locationStatusLabel.Text = "❌ Could not get location";
                    locationStatusLabel.TextColor = Colors.Red;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GetLocation error: {ex.Message}");
            if (locationStatusLabel != null)
            {
                locationStatusLabel.Text = $"Error: {ex.Message}";
                locationStatusLabel.TextColor = Colors.Red;
            }
        }
        finally
        {
            if (getLocationButton != null) getLocationButton.IsEnabled = true;
        }
    }

    private async void OnFindNearbyClicked(object sender, EventArgs e)
    {
        if (_currentLocation == null)
        {
            await DisplayAlert("No Location", "Please tap 📍 first to get your location.", "OK");
            return;
        }

        try
        {
            if (findPlacesButton != null) findPlacesButton.IsEnabled = false;
            if (placesListLabel != null)
            {
                placesListLabel.Text = "Searching...";
                placesListLabel.TextColor = Colors.DarkOrange;
            }

            if (_locationService == null)
                _locationService = new LocationService();

            _nearbyPlaces = await _locationService.GetNearbyPlacesAsync(_currentLocation);

            if (_nearbyPlaces != null && _nearbyPlaces.Count > 0)
            {
                string placesText = string.Empty;
                for (int i = 0; i < _nearbyPlaces.Count; i++)
                {
                    var place = _nearbyPlaces[i];
                    placesText += $"{i + 1}. {place.FeatureName}\n   📍 {place.Thoroughfare}\n\n";
                }

                if (placesListLabel != null)
                {
                    placesListLabel.Text = placesText.TrimEnd();
                    placesListLabel.TextColor = Colors.Black;
                }

                if (speakLocationButton != null) speakLocationButton.IsEnabled = true;

                await DisplayAlert("Places Found",
                    $"Found {_nearbyPlaces.Count} restaurants near your location!", "OK");
            }
            else
            {
                if (placesListLabel != null)
                {
                    placesListLabel.Text = "No restaurants found nearby";
                    placesListLabel.TextColor = Colors.Red;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"FindNearby error: {ex.Message}");
            if (placesListLabel != null)
            {
                placesListLabel.Text = $"Error: {ex.Message}";
                placesListLabel.TextColor = Colors.Red;
            }
        }
        finally
        {
            if (findPlacesButton != null) findPlacesButton.IsEnabled = true;
        }
    }

    private async void OnSpeakLocationClicked(object sender, EventArgs e)
    {
        string message = string.Empty;

        if (_currentLocation != null)
        {
            message += $"Your location is latitude {_currentLocation.Latitude:F2}, longitude {_currentLocation.Longitude:F2}. ";
        }

        if (_nearbyPlaces != null && _nearbyPlaces.Count > 0)
        {
            message += $"Nearby restaurants: ";
            foreach (var place in _nearbyPlaces.Take(3))
            {
                message += $"{place.FeatureName}. ";
            }
        }
        else
        {
            message += "No restaurants found. Please tap the find nearby button first.";
        }

        if (string.IsNullOrEmpty(message))
        {
            message = "Please get your location first by tapping the location button.";
        }

        await SpeakMessageAsync(message);
    }

    private void OnStopSpeakingClicked(object sender, EventArgs e)
    {
        try
        {
            _ttsCancellationTokenSource?.Cancel();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"StopSpeaking error: {ex.Message}");
        }
    }

    private async Task SpeakMessageAsync(string message)
    {
        try
        {
            _ttsCancellationTokenSource?.Cancel();
            _ttsCancellationTokenSource = new CancellationTokenSource();

            await TextToSpeech.Default.SpeakAsync(message,
                new SpeechOptions { Volume = 1.0f, Pitch = 1.0f },
                _ttsCancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            // User cancelled
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"TTS error: {ex.Message}");
        }
    }
}