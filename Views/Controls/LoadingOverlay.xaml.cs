namespace FoodSnap.Views.Controls;

public partial class LoadingOverlay : ContentView
{
    public LoadingOverlay()
    {
        InitializeComponent();
        IsVisible = false;
    }

    public void Show(string text = "Loading...")
    {
        loadingTextLabel.Text = text;
        progressBar.IsVisible = false;
        IsVisible = true;
    }

    public void ShowProgress(double progress, string text = "Processing...")
    {
        loadingTextLabel.Text = text;
        progressBar.Progress = progress;
        progressBar.IsVisible = true;
        IsVisible = true;
    }

    public void Hide()
    {
        IsVisible = false;
    }
}