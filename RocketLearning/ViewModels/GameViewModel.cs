using System;
using Avalonia.Input;
using Avalonia.Threading;
using RocketLearning.Game;

namespace RocketLearning.ViewModels;

public class GameViewModel : ViewModelBase
{
    private readonly GameState _state = new GameState();
    public (double X, double Y, double Width, double Height) PlatformRect => _state.Terrain.GetPlatformRect();
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
