using Avalonia.Controls;
using RocketLearning.ViewModels;

namespace RocketLearning.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
