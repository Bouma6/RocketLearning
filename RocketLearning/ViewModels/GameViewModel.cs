using System;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Threading;
using RocketLearning.Game;
using RocketLearning.Agent;

namespace RocketLearning.ViewModels;

public class GameViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly GameState _state = new();
    private readonly IAgent _agent;
    private readonly DispatcherTimer _timer;

    public ICommand ResetCommand { get; }
    public ICommand BackToMenuCommand { get; }

    public double RocketPositionX => _state.Rocket.PositionX - 80;
    public double RocketPositionY => _state.Rocket.PositionY - 80;
    public double RocketAngle => _state.Rocket.Angle;
    public double Score => _state.Score;
    public double BestScore => _main.SessionBestScore;
    public string FormattedTime => TimeSpan.FromSeconds(_state.Time).ToString(@"mm\:ss\.ff");

    public bool GameOver => _state.GameOver;

    public double PlatformX => _state.Terrain.PlatformX;
    public double PlatformY => _state.Terrain.PlatformY;
    public double PlatformWidth => _state.Terrain.PlatformWidth;
    public double PlatformHeight => _state.Terrain.PlatformHeight;

    public double LeftFlagX => PlatformX - 55;
    public double RightFlagX => PlatformX + PlatformWidth - 60;
    public double FlagY => PlatformY - 30;

    public GameViewModel(MainWindowViewModel main, bool useNeat = false)
    {
        _main = main;
        //NeatLoader - have to be implemented
        //Load from probably Master Trainer or from somewhere where the best NN will be store idk 
        //_agent = useNeat ? new NeatAgent(NeatLoader.LoadBestNetwork()) : new HumanAgent();
        _agent = new HumanAgent();
        _state.OnStateChanged += () =>
        {
            OnPropertyChanged(nameof(RocketPositionX));
            OnPropertyChanged(nameof(RocketPositionY));
            OnPropertyChanged(nameof(RocketAngle));
            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(FormattedTime));
            OnPropertyChanged(nameof(GameOver));
            OnPropertyChanged(nameof(BestScore));
            if (_state.GameOver)
                _main.ReportScore(_state.Score);
        };

        ResetCommand = new RelayCommand(() => _state.Reset());
        BackToMenuCommand = new RelayCommand(() => _main.CurrentView = new MenuViewModel(_main));

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _timer.Tick += (_, _) =>
        {
            var input = _agent.Decide(_state);
            _state.Tick(input, 1.0 / 60.0);
        };
        _timer.Start();
    }

    public void OnKeyPressed(KeyEventArgs e)
    {
        if (_agent is HumanAgent human)
            human.OnKeyPressed(e);
    }
}
