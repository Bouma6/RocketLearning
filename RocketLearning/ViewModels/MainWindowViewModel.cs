using System.Windows.Input;

namespace RocketLearning.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private object _currentView = null!;
    public object CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }
    public ICommand StartCommand { get; }

    public MainWindowViewModel()
    {
        StartCommand = new RelayCommand(StartGame);
        CurrentView = new MenuViewModel(this);
    }

    private void StartGame()
    {
        CurrentView = new GameViewModel(this);
    }
}