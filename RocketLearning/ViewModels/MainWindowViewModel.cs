using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RocketLearning.ReinforcementLearning;

namespace RocketLearning.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const string BestScorePath = "BestScore.txt";
    public readonly MasterTrainer Trainer = new(network => Agent.GameEvaluator.EvaluateNetwork(network));

    [ObservableProperty]
    private object currentView = null!;

    [ObservableProperty]
    private double sessionBestScore = 0;

    [ObservableProperty]
    private double allTimeBestScore;

    [ObservableProperty]
    private bool isTrainingPopupVisible;

    public TrainingProgressViewModel TrainingProgress { get; } = new();

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

    public void LoadSavedNetwork()
    {
        Trainer.LoadBestGenome();
    }

    public async void StartTraining()
    {
        IsTrainingPopupVisible = true;
        TrainingProgress.Progress = 0;

        await Task.Run(() =>
        {
            Trainer.Run(progress =>
            {
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    TrainingProgress.Progress = progress;
                });
            });
        });

        IsTrainingPopupVisible = false;
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
