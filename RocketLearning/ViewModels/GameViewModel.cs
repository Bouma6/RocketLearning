using System;
using Avalonia.Input;
using Avalonia.Threading;
using RocketLearning.Game;

namespace RocketLearning.ViewModels;

public class GameViewModel : ViewModelBase
{
    private readonly GameState _state = new GameState();
    public double PlatformX => _state.Terrain.PlatformX;
    public double PlatformY => _state.Terrain.PlatformY;
    public double PlatformWidth => _state.Terrain.PlatformWidth;
    public double PlatformHeight => _state.Terrain.PlatformHeight;
    public GameViewModel()
    {
        _state.OnStateChanged += () =>
        {
            OnPropertyChanged(nameof(RocketPositionX));
            OnPropertyChanged(nameof(RocketPositionY));
            OnPropertyChanged(nameof(RocketAngle));
        };
    }

    public double RocketPositionX => _state.Rocket.PositionX - 80;
    public double RocketPositionY => _state.Rocket.PositionY - 80;
    public double RocketAngle => _state.Rocket.Angle;

    public void OnKeyPressed(KeyEventArgs e)
    {
        if (e.Key == Key.Left)
            _state.Rocket.LeftMotor();
        if (e.Key == Key.Right)
            _state.Rocket.RightMotor();
    }
}
