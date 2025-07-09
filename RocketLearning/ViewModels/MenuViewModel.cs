using System.Windows.Input;

namespace RocketLearning.ViewModels;

public class MenuViewModel(MainWindowViewModel _main) : ViewModelBase
{
    public double PersistentBestScore => _main.AllTimeBestScore;
    public double SessionBestScore => _main.SessionBestScore;

    public ICommand StartCommand { get; } = new RelayCommand(() => _main.CurrentView = new GameViewModel(_main));
}