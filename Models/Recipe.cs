namespace FoodSnap.Models;

public class Recipe
{
    public string Name { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public int Calories { get; set; }
    public int PrepTime { get; set; }
    public string Icon { get; set; } = string.Empty;
}