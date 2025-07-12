using System.Windows.Input;

namespace RocketLearning.ViewModels;

public class MenuViewModel(MainWindowViewModel main) : ViewModelBase
{
    private readonly double _persistentBestScore = main.AllTimeBestScore;
    private readonly double _sessionBestScore = main.SessionBestScore;

    public double PersistentBestScore => _persistentBestScore;
    public double SessionBestScore => _sessionBestScore;

    public ICommand StartCommand { get; } = new RelayCommand(() => main.CurrentView = new GameViewModel(main, false));
    public ICommand StartCommandNeat { get; } = new RelayCommand(() => main.CurrentView = new GameViewModel(main, true));
    public ICommand StartTrainingCommand { get; } = new RelayCommand(main.StartTraining);
}