using FoodSnap.Models;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel;

namespace FoodSnap.Views;

public partial class ScanPage : ContentPage
{
    private FoodItem? _lastScannedFoodItem;
    private CancellationTokenSource? _ttsCancellationTokenSource;

    public ScanPage()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ScanPage init error: {ex.Message}");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            // Reset UI state when page appears
            if (resultLabel != null)
            {
                resultLabel.Text = "📷 Take photo or 🎲 simulate scan";
                resultLabel.TextColor = Colors.Gray;
            }
            if (speakButton != null)
                speakButton.IsEnabled = false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ScanPage OnAppearing error: {ex.Message}");
        }
    }

    private async void OnCapturePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (cameraStatus != PermissionStatus.Granted)
            {
                cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (cameraStatus != PermissionStatus.Granted)
            {
                if (resultLabel != null)
                {
                    resultLabel.Text = "❌ Camera permission denied";
                    resultLabel.TextColor = Colors.Red;
                }
                await DisplayAlert("Permission Required", "Camera access is needed to scan barcodes.", "OK");
                return;
            }

            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo == null)
            {
                if (resultLabel != null)
                    resultLabel.Text = "Photo capture cancelled";
                return;
            }

            if (resultLabel != null)
            {
                resultLabel.Text = "Processing...";
                resultLabel.TextColor = Colors.DarkOrange;
            }

            await Task.Delay(1500);

            string simulatedBarcode = "5901234123457";
            await ProcessBarcode(simulatedBarcode);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Camera error: {ex.Message}");
            if (resultLabel != null)
            {
                resultLabel.Text = "Camera error";
                resultLabel.TextColor = Colors.Red;
            }
            await DisplayAlert("Error", $"Camera error: {ex.Message}", "OK");
        }
    }

    private async void OnSimulateScanClicked(object sender, EventArgs e)
    {
        await ProcessBarcode("5901234123457");
    }

    private async Task ProcessBarcode(string barcode)
    {
        try
        {
            if (captureButton != null) captureButton.IsEnabled = false;
            if (simulateButton != null) simulateButton.IsEnabled = false;
            if (resultLabel != null)
            {
                resultLabel.Text = $"Scanning: {barcode}...";
                resultLabel.TextColor = Colors.DarkOrange;
            }

            await Task.Delay(500);

            var foodItem = GetFoodItemByBarcode(barcode);

            if (foodItem != null)
            {
                _lastScannedFoodItem = foodItem;

                if (resultLabel != null)
                {
                    resultLabel.Text = $"✅ {foodItem.Name}: {foodItem.Calories} cal";
                    resultLabel.TextColor = Colors.Green;
                }

                if (speakButton != null) speakButton.IsEnabled = true;

                // Vibration feedback
                VibrateShort();

                await DisplayAlert("Scan Successful",
                    $"Food: {foodItem.Name}\n\n" +
                    $"Calories: {foodItem.Calories} kcal\n" +
                    $"Protein: {foodItem.Protein}g\n" +
                    $"Carbs: {foodItem.Carbs}g\n" +
                    $"Fat: {foodItem.Fat}g",
                    "OK");
            }
            else
            {
                _lastScannedFoodItem = null;
                if (resultLabel != null)
                {
                    resultLabel.Text = "❌ Barcode not found";
                    resultLabel.TextColor = Colors.Red;
                }
                if (speakButton != null) speakButton.IsEnabled = false;
                await DisplayAlert("Not Found", $"Barcode '{barcode}' not recognized.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ProcessBarcode error: {ex.Message}");
            if (resultLabel != null)
            {
                resultLabel.Text = "Error";
                resultLabel.TextColor = Colors.Red;
            }
        }
        finally
        {
            if (captureButton != null) captureButton.IsEnabled = true;
            if (simulateButton != null) simulateButton.IsEnabled = true;
        }
    }

    private void VibrateShort()
    {
        try
        {
            Vibration.Default.Vibrate(100);
        }
        catch (FeatureNotSupportedException)
        {
            Debug.WriteLine("Vibration not supported on this device");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Vibration error: {ex.Message}");
        }
    }

    private async void OnSpeakLastResultClicked(object sender, EventArgs e)
    {
        if (_lastScannedFoodItem == null)
        {
            await SpeakMessageAsync("No scan result available. Please scan a barcode first.");
            return;
        }

        string message = $"{_lastScannedFoodItem.Name}. {_lastScannedFoodItem.Calories} calories. " +
                        $"{_lastScannedFoodItem.Protein} grams protein. {_lastScannedFoodItem.Carbs} grams carbs. " +
                        $"{_lastScannedFoodItem.Fat} grams fat.";

        await SpeakMessageAsync(message);
    }

    private void OnStopSpeakingClicked(object sender, EventArgs e)
    {
        try
        {
            _ttsCancellationTokenSource?.Cancel();
            if (resultLabel != null)
            {
                resultLabel.Text = "🔇 Speech stopped";
                resultLabel.TextColor = Colors.DarkOrange;
            }

            Task.Delay(1000).ContinueWith(_ =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (resultLabel != null)
                    {
                        if (_lastScannedFoodItem != null)
                            resultLabel.Text = $"✅ {_lastScannedFoodItem.Name}: {_lastScannedFoodItem.Calories} cal";
                        else
                            resultLabel.Text = "📷 Take photo or 🎲 simulate scan";
                    }
                });
            });
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

    private FoodItem? GetFoodItemByBarcode(string barcode)
    {
        var database = new Dictionary<string, FoodItem>
        {
            { "5901234123457", new FoodItem { Name = "Apple", Calories = 95, Protein = 0.5, Carbs = 25, Fat = 0.3 } },
            { "5901234123458", new FoodItem { Name = "Banana", Calories = 105, Protein = 1.3, Carbs = 27, Fat = 0.4 } },
            { "5901234123459", new FoodItem { Name = "Chicken Breast", Calories = 165, Protein = 31, Carbs = 0, Fat = 3.6 } }
        };
        return database.GetValueOrDefault(barcode);
    }
}