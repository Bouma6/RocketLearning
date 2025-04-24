using Avalonia.Input;
namespace RocketLearning.ViewModels;
using RocketLearning.Game;

public class GameViewModel : ViewModelBase
{
    private Rocket Rocket { get; }

    public GameViewModel()
    {
        Rocket = new Rocket { PositionY = 100,PositionX = 100, Angle = 90 };
    }

    public void OnKeyPressed(KeyEventArgs e)
    {
        if (e.Key == Key.Left)
            Rocket.LeftMotor();
        if (e.Key == Key.Right)
            Rocket.RightMotor();
        if (e.Key == Key.Up)
            Rocket.RotateLeft();
        if (e.Key == Key.Down)
            Rocket.RotateRight();
        OnPropertyChanged(nameof(RocketPositionY));
        OnPropertyChanged(nameof(RocketPositionX));
        OnPropertyChanged(nameof(RocketAngle));
    }

    public double RocketPositionY => Rocket.PositionY;
    public double RocketPositionX => Rocket.PositionX;
    public double RocketAngle => Rocket.Angle;
}