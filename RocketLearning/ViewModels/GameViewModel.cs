using System;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Threading;
using RocketLearning.Game;

namespace RocketLearning.ViewModels;

public class GameViewModel : ViewModelBase
{
    private readonly GameState _state = new GameState();
    public bool Crashed => _state.GameOver;
    public double Score => _state.Score;
    public double PlatformX => _state.Terrain.PlatformX;
    public double PlatformY => _state.Terrain.PlatformY;
    public double PlatformWidth => _state.Terrain.PlatformWidth;
    public double PlatformHeight => _state.Terrain.PlatformHeight;
    public double LeftFlagX => _state.Terrain.PlatformX -55;
    public double FlagY => _state.Terrain.PlatformY-30;
    public double RightFlagX => _state.Terrain.PlatformX +PlatformWidth - 60;
    public ICommand ResetCommand { get; }
    public ICommand BackToMenuCommand { get; }

    private readonly MainWindowViewModel _main;

    public GameViewModel(MainWindowViewModel main)
    {
        _main = main;
    
        _state.OnStateChanged += () =>
        {
            OnPropertyChanged(nameof(RocketPositionX));
            OnPropertyChanged(nameof(RocketPositionY));
            OnPropertyChanged(nameof(RocketAngle));
            OnPropertyChanged(nameof(Crashed));
            OnPropertyChanged(nameof(Score));
        };

        ResetCommand = new RelayCommand(() => _state.Reset());
        BackToMenuCommand = new RelayCommand(() => _main.CurrentView = new MenuViewModel(_main));
    }

    public double RocketPositionX => _state.Rocket.PositionX - 80;
    public double RocketPositionY => _state.Rocket.PositionY - 80;
    public double RocketAngle => _state.Rocket.Angle;

    public void OnKeyPressed(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                _state.SetInput(RocketInput.Left);
                break;
            case Key.Right:
                _state.SetInput(RocketInput.Right);
                break;
        }
    }
}
