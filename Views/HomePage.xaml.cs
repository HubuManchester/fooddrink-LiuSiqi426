using FoodSnap.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace FoodSnap.Views;

public partial class HomePage : ContentPage
{
    private List<FoodItem> _allFoods = new();
    private List<Recipe> _allRecipes = new();
    private List<object> _searchResults = new();

    public HomePage()
    {
        InitializeComponent();
        LoadData();
        SetupRecommendationsView();
        SetupPopularFoodsView();
        SetupSearchResultsView();
    }

    private void LoadData()
    {
        // Load foods
        _allFoods = new List<FoodItem>
        {
            new FoodItem { Name = "Apple", Calories = 95, Protein = 0.5, Carbs = 25, Fat = 0.3, Icon = "🍎", Description = "A crisp, sweet fruit rich in fiber and vitamin C." },
            new FoodItem { Name = "Banana", Calories = 105, Protein = 1.3, Carbs = 27, Fat = 0.4, Icon = "🍌", Description = "Rich in potassium and great for energy." },
            new FoodItem { Name = "Chicken Breast", Calories = 165, Protein = 31, Carbs = 0, Fat = 3.6, Icon = "🍗", Description = "Lean protein, perfect for muscle building." },
            new FoodItem { Name = "Salmon", Calories = 208, Protein = 20, Carbs = 0, Fat = 13, Icon = "🐟", Description = "Rich in omega-3 fatty acids." },
            new FoodItem { Name = "Broccoli", Calories = 55, Protein = 3.7, Carbs = 11, Fat = 0.6, Icon = "🥦", Description = "High in fiber and vitamin C." },
            new FoodItem { Name = "Avocado", Calories = 234, Protein = 2.9, Carbs = 12, Fat = 21, Icon = "🥑", Description = "Healthy fats and creamy texture." },
            new FoodItem { Name = "Egg", Calories = 72, Protein = 6.3, Carbs = 0.4, Fat = 4.8, Icon = "🥚", Description = "Complete protein source." },
            new FoodItem { Name = "Oatmeal", Calories = 158, Protein = 5.5, Carbs = 27, Fat = 3.2, Icon = "🥣", Description = "High in fiber, great for breakfast." },
            new FoodItem { Name = "Greek Yogurt", Calories = 100, Protein = 10, Carbs = 6, Fat = 5, Icon = "🥛", Description = "Probiotic rich and creamy." },
             new FoodItem { Name = "Blueberries", Calories = 57, Protein = 0.7, Carbs = 14, Fat = 0.3, Icon = "🍇", Description = "Antioxidant rich superfood." }
        };

        // Load recipes
        _allRecipes = new List<Recipe>
        {
            new Recipe { Name = "Avocado Toast", Calories = 250, Icon = "🥑", PrepTime = 5, Ingredients = "Bread, Avocado, Salt, Pepper", Instructions = "Toast bread, mash avocado, spread, season." },
            new Recipe { Name = "Berry Smoothie Bowl", Calories = 300, Icon = "🥣", PrepTime = 5, Ingredients = "Banana, Berries, Yogurt", Instructions = "Blend ingredients, pour into bowl, top with granola." },
            new Recipe { Name = "Quinoa Salad", Calories = 280, Icon = "🥗", PrepTime = 15, Ingredients = "Quinoa, Cucumber, Tomato, Feta", Instructions = "Cook quinoa, mix with chopped veggies, add feta." },
            new Recipe { Name = "Banana Pancakes", Calories = 350, Icon = "🥞", PrepTime = 10, Ingredients = "Banana, Eggs, Oats", Instructions = "Mash banana, mix with eggs and oats, cook in pan." }
        };
    }

    private void SetupRecommendationsView()
    {
        var recommendations = new List<FoodItem>
        {
            _allFoods.First(f => f.Name == "Apple"),
            _allFoods.First(f => f.Name == "Salmon"),
            _allFoods.First(f => f.Name == "Avocado"),
            _allFoods.First(f => f.Name == "Blueberries")
        };
        recommendationsView.ItemsSource = recommendations;
        recommendationsView.ItemTemplate = CreateFoodItemTemplate(true);
    }

    private void SetupPopularFoodsView()
    {
        var popular = new List<FoodItem>
        {
            _allFoods.First(f => f.Name == "Banana"),
            _allFoods.First(f => f.Name == "Chicken Breast"),
            _allFoods.First(f => f.Name == "Greek Yogurt"),
            _allFoods.First(f => f.Name == "Oatmeal"),
            _allFoods.First(f => f.Name == "Broccoli"),
            _allFoods.First(f => f.Name == "Egg")
        };
        popularFoodsView.ItemsSource = popular;
        popularFoodsView.ItemTemplate = CreateFoodItemTemplateWithArrow();
    }

    private void SetupSearchResultsView()
    {
        searchResultsView.ItemTemplate = CreateSearchResultTemplate();
    }

    private DataTemplate CreateFoodItemTemplate(bool isRecommendation)
    {
        return new DataTemplate(() =>
        {
            var layout = new VerticalStackLayout { Spacing = 5, HorizontalOptions = LayoutOptions.Center };

            var iconLabel = new Label { FontSize = 32, HorizontalOptions = LayoutOptions.Center };
            iconLabel.SetBinding(Label.TextProperty, "Icon");

            var nameLabel = new Label { FontSize = 12, HorizontalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 2 };
            nameLabel.SetBinding(Label.TextProperty, "Name");

            var calLabel = new Label { FontSize = 10, TextColor = Colors.Gray, HorizontalOptions = LayoutOptions.Center };
            calLabel.SetBinding(Label.TextProperty, new Binding("Calories", stringFormat: "{0} cal"));

            layout.Add(iconLabel);
            layout.Add(nameLabel);
            layout.Add(calLabel);

            var frame = new Frame { CornerRadius = 10, Margin = 5, Padding = 10, BackgroundColor = Colors.White };
            frame.Content = layout;

            // Add tap gesture for better sensitivity
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) => {
                var food = frame.BindingContext as FoodItem;
                if (food != null) ShowFoodDetail(food);
            };
            frame.GestureRecognizers.Add(tapGesture);

            return frame;
        });
    }

    private DataTemplate CreateFoodItemTemplateWithArrow()
    {
        return new DataTemplate(() =>
        {
            var grid = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition { Width = GridLength.Auto }, new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }, new ColumnDefinition { Width = GridLength.Auto } }, ColumnSpacing = 10 };

            var iconLabel = new Label { FontSize = 24, VerticalOptions = LayoutOptions.Center };
            iconLabel.SetBinding(Label.TextProperty, "Icon");

            var nameLayout = new VerticalStackLayout();
            var nameLabel = new Label { FontSize = 14, FontAttributes = FontAttributes.Bold };
            nameLabel.SetBinding(Label.TextProperty, "Name");
            nameLayout.Add(nameLabel);

            var arrowLabel = new Label { Text = "→", FontSize = 18, VerticalOptions = LayoutOptions.Center, TextColor = Colors.Gray };

            grid.Add(iconLabel, 0, 0);
            grid.Add(nameLayout, 1, 0);
            grid.Add(arrowLabel, 2, 0);

            var frame = new Frame { CornerRadius = 10, Margin = new Thickness(0, 5), Padding = 10, BackgroundColor = Colors.White };
            frame.Content = grid;

            // Add tap gesture for better sensitivity
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) => {
                var food = frame.BindingContext as FoodItem;
                if (food != null) ShowFoodDetail(food);
            };
            frame.GestureRecognizers.Add(tapGesture);

            return frame;
        });
    }

    private DataTemplate CreateSearchResultTemplate()
    {
        return new DataTemplate(() =>
        {
            var grid = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition { Width = GridLength.Auto }, new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }, new ColumnDefinition { Width = GridLength.Auto } }, ColumnSpacing = 10 };

            var iconLabel = new Label { FontSize = 24, VerticalOptions = LayoutOptions.Center };
            iconLabel.SetBinding(Label.TextProperty, "Icon");

            var nameLayout = new VerticalStackLayout();
            var nameLabel = new Label { FontSize = 14, FontAttributes = FontAttributes.Bold };
            nameLabel.SetBinding(Label.TextProperty, "Name");

            var calLabel = new Label { FontSize = 12, TextColor = Colors.Gray };
            calLabel.SetBinding(Label.TextProperty, new Binding("Calories", stringFormat: "{0} cal"));

            nameLayout.Add(nameLabel);
            nameLayout.Add(calLabel);

            var arrowLabel = new Label { Text = "→", FontSize = 18, VerticalOptions = LayoutOptions.Center, TextColor = Colors.Gray };

            grid.Add(iconLabel, 0, 0);
            grid.Add(nameLayout, 1, 0);
            grid.Add(arrowLabel, 2, 0);

            var frame = new Frame { CornerRadius = 10, Margin = new Thickness(0, 5), Padding = 10, BackgroundColor = Colors.White };
            frame.Content = grid;

            // Add tap gesture for better sensitivity
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) => {
                var item = frame.BindingContext;
                if (item is FoodItem food)
                {
                    await ShowFoodDetail(food);
                }
                else if (item is Recipe recipe)
                {
                    await ShowRecipeDetail(recipe);
                }
            };
            frame.GestureRecognizers.Add(tapGesture);

            return frame;
        });
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            searchResultsLayout.IsVisible = false;
            recommendationsLayout.IsVisible = true;
            _searchResults.Clear();
        }
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        var query = searchEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(query))
        {
            await DisplayAlert("Search", "Please enter a food or recipe name.", "OK");
            return;
        }
        PerformSearch(query);
    }

    private void PerformSearch(string query)
    {
        _searchResults.Clear();
        var matchedFoods = _allFoods.Where(f => f.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        var matchedRecipes = _allRecipes.Where(r => r.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        _searchResults.AddRange(matchedFoods);
        _searchResults.AddRange(matchedRecipes);

        if (_searchResults.Count > 0)
        {
            searchResultsView.ItemsSource = _searchResults;
            searchResultsLayout.IsVisible = true;
            recommendationsLayout.IsVisible = false;
        }
        else
        {
            DisplayAlert("Search", $"No results found for '{query}'.", "OK");
        }
    }

    // Removed SelectionChanged handlers - using TapGestureRecognizer instead
    private void OnSearchResultSelected(object sender, SelectionChangedEventArgs e)
    {
        // Clear selection only, actual handling done by tap gesture
        searchResultsView.SelectedItem = null;
    }

    private void OnRecommendationSelected(object sender, SelectionChangedEventArgs e)
    {
        recommendationsView.SelectedItem = null;
    }

    private void OnPopularFoodSelected(object sender, SelectionChangedEventArgs e)
    {
        popularFoodsView.SelectedItem = null;
    }

    private async Task ShowFoodDetail(FoodItem food)
    {
        string message = $"📊 {food.Name}\n\n" +
                        $"🔥 Calories: {food.Calories} kcal\n" +
                        $"💪 Protein: {food.Protein}g\n" +
                        $"🍚 Carbs: {food.Carbs}g\n" +
                        $"🥑 Fat: {food.Fat}g\n\n" +
                        $"📝 {food.Description}";

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await DisplayAlert($"🍽️ {food.Name}", message, "OK");
        });
    }

    private async Task ShowRecipeDetail(Recipe recipe)
    {
        string message = $"📖 {recipe.Name}\n\n" +
                        $"🔥 Calories: {recipe.Calories} kcal\n" +
                        $"⏱️ Prep Time: {recipe.PrepTime} min\n\n" +
                        $"🥗 Ingredients:\n{recipe.Ingredients}\n\n" +
                        $"📝 Instructions:\n{recipe.Instructions}";

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await DisplayAlert($"🍳 {recipe.Name}", message, "OK");
        });
    }

    private async void OnStartScanningClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ScanPage");
    }
}