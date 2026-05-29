using FoodSnap.Models;
using Microsoft.Maui.Devices.Sensors;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel;

namespace FoodSnap.Views;

public partial class ShakePage : ContentPage
{
    private readonly Random _random = new();
    private List<Recipe> _recipes = new();
    private Recipe? _currentRecipe;
    private CancellationTokenSource? _ttsCancellationTokenSource;

    public ShakePage()
    {
        try
        {
            InitializeComponent();
            LoadRecipes();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ShakePage init error: {ex.Message}");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (!Accelerometer.IsMonitoring)
            {
                Accelerometer.Start(SensorSpeed.Game);
            }
            Accelerometer.ShakeDetected += OnShakeDetected;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ShakePage OnAppearing error: {ex.Message}");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        try
        {
            Accelerometer.ShakeDetected -= OnShakeDetected;
            if (Accelerometer.IsMonitoring)
            {
                Accelerometer.Stop();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ShakePage OnDisappearing error: {ex.Message}");
        }
    }

    private void LoadRecipes()
    {
        _recipes = new List<Recipe>
        {
            new Recipe { Name = "Avocado Toast", Ingredients = "2 slices bread, 1 avocado, Salt, Pepper", Instructions = "Toast bread. Mash avocado. Spread on toast. Season." },
            new Recipe { Name = "Berry Smoothie Bowl", Ingredients = "1 banana, 1 cup berries, 1/2 cup yogurt", Instructions = "Blend all ingredients. Pour into bowl. Top with granola." },
            new Recipe { Name = "Quinoa Salad", Ingredients = "1 cup quinoa, Veggies, Feta cheese", Instructions = "Cook quinoa. Mix with veggies. Add feta. Serve chilled." },
            new Recipe { Name = "Banana Pancakes", Ingredients = "2 bananas, 2 eggs, 1/2 cup oats", Instructions = "Mash bananas. Mix with eggs and oats. Cook in pan." },
            new Recipe { Name = "Carrot Ginger Soup", Ingredients = "4 carrots, 1 onion, Ginger, Broth", Instructions = "Sauté onion. Add carrots and broth. Blend until smooth." },
            new Recipe { Name = "Apple Oatmeal", Ingredients = "1 cup oats, 2 cups milk, 1 apple, Cinnamon", Instructions = "Cook oats. Add apple and cinnamon. Stir in honey." },
            new Recipe { Name = "Veggie Wrap", Ingredients = "Tortilla, Hummus, Veggies, Spinach", Instructions = "Spread hummus. Layer veggies. Roll tightly. Cut in half." },
            new Recipe { Name = "Lemon Herb Chicken", Ingredients = "2 chicken breasts, Lemon, Herbs", Instructions = "Marinate chicken. Cook in pan. Serve with vegetables." }
        };
    }

    private void OnShakeDetected(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                DisplayRandomRecipe();
                VibrateShort();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ShakeDetected error: {ex.Message}");
            }
        });
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

    private void DisplayRandomRecipe()
    {
        if (_recipes == null || _recipes.Count == 0) return;

        int index = _random.Next(_recipes.Count);
        _currentRecipe = _recipes[index];

        if (recipeNameLabel != null)
            recipeNameLabel.Text = _currentRecipe.Name;
        if (ingredientsLabel != null)
            ingredientsLabel.Text = _currentRecipe.Ingredients;
        if (instructionsLabel != null)
            instructionsLabel.Text = _currentRecipe.Instructions;
        if (speakButton != null)
            speakButton.IsEnabled = true;

        if (instructionLabel != null)
        {
            instructionLabel.Text = "🫨 Shake again for another recipe!";
            instructionLabel.TextColor = Colors.Green;
        }

        Task.Delay(2000).ContinueWith(_ =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (instructionLabel != null)
                {
                    instructionLabel.Text = "Shake your phone to get a random recipe!";
                    instructionLabel.TextColor = Colors.DarkOrange;
                }
            });
        });
    }

    private async void OnSpeakClicked(object? sender, EventArgs e)
    {
        if (_currentRecipe == null)
        {
            await SpeakMessageAsync("No recipe available. Please shake your phone first.");
            return;
        }

        string message = $"{_currentRecipe.Name}. Ingredients: {_currentRecipe.Ingredients}. Instructions: {_currentRecipe.Instructions}";
        await SpeakMessageAsync(message);
    }

    private void OnStopSpeakingClicked(object? sender, EventArgs e)
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