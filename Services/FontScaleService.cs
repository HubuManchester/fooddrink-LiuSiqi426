namespace FoodSnap.Services;

public static class FontScaleService
{
    private const string FontScaleKey = "font_scale";

    public static float GetCurrentScale()
    {
        var saved = Preferences.Get(FontScaleKey, 1.0f);
        return saved;
    }

    public static void SetScale(float scale)
    {
        Preferences.Set(FontScaleKey, scale);
    }

    public static double GetScaledSize(double originalSize)
    {
        return originalSize * GetCurrentScale();
    }
}