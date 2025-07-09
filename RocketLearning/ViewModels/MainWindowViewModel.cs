using System.IO;
using System.Windows.Input;

namespace RocketLearning.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private object _currentView = null!;
    private const string BestScorePath = "BestScore.txt";
    public double SessionBestScore { get; private set; } = 0;
    public double AllTimeBestScore { get; private set; }
    public ICommand StartCommand { get; }
    public MainWindowViewModel()
    {
        StartCommand = new RelayCommand(StartGame);
        CurrentView = new MenuViewModel(this);
        LoadBestScore();
    }
    private void LoadBestScore()
    {
        if (File.Exists(BestScorePath) && double.TryParse(File.ReadAllText(BestScorePath), out double score))
            AllTimeBestScore = score;
    }

    private void SaveBestScore()
    {
        File.WriteAllText(BestScorePath, AllTimeBestScore.ToString("F2"));
    }
    public object CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public void ReportScore(double score)
    {
        if (score > SessionBestScore)
            SessionBestScore = score;

        if (score > AllTimeBestScore)
        {
            AllTimeBestScore = score;
            SaveBestScore();
        }
    }
    private void StartGame()
    {
        CurrentView = new GameViewModel(this);
    }
}