using Avalonia.Input;
namespace RocketLearning.ViewModels;
using Game;

public class GameViewModel : ViewModelBase
{
    private Rocket Rocket { get; }

    public GameViewModel()
    {
        Rocket = new Rocket { PositionY = 0,PositionX = 320, Angle = 0 };
    }

    public void OnKeyPressed(KeyEventArgs e)
    {
        if (e.Key == Key.Left)
            Rocket.LeftMotor();
        if (e.Key == Key.Right)
            Rocket.RightMotor();
        OnPropertyChanged(nameof(RocketPositionY));
        OnPropertyChanged(nameof(RocketPositionX));
        OnPropertyChanged(nameof(RocketAngle));
    }

    public double RocketPositionY => Rocket.PositionY;
    public double RocketPositionX => Rocket.PositionX;
    public double RocketAngle => Rocket.Angle;
}