using System;

using Avalonia.Input;
using Avalonia.Threading;
using RocketLearning.Game;

namespace RocketLearning.ViewModels;

public class GameViewModel : ViewModelBase
{
    private Rocket Rocket { get; }

    private readonly DispatcherTimer _timer;

    public GameViewModel()
    {
        Rocket = new Rocket();

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        _timer.Tick += (_, _) =>
        {
            Rocket.Tick();
            OnPropertyChanged(nameof(RocketPositionX));
            OnPropertyChanged(nameof(RocketPositionY));
            OnPropertyChanged(nameof(RocketAngle));
        };
        _timer.Start();
    }

    public double RocketPositionX => Rocket.PositionX;
    public double RocketPositionY => Rocket.PositionY;
    public double RocketAngle => Rocket.Angle;

    public void OnKeyPressed(KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            Rocket.LeftMotor();
        }
        if (e.Key == Key.Right)
        {
            Rocket.RightMotor();
        }
    }
}