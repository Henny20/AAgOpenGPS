using CommunityToolkit.Mvvm.ComponentModel;

namespace AgOpenGPS.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    [ObservableProperty]
    private SolidColorBrush _buttonColor = new SolidColorBrush(Color.FromRgb(243, 243, 243));
    
    [ObservableProperty]
    private bool _buttonVisible = true;
    
}
