using Avalonia.Input;
namespace RocketLearning.ViewModels;
using RocketLearning.Game;

public class GameViewModel : ViewModelBase
{
    private Rocket Rocket { get; }

    public GameViewModel()
    {
        Rocket = new Rocket { PositionY = 100,PositionX = 100 };
    }

    public void OnKeyPressed(KeyEventArgs e)
    {
        if (e.Key == Key.Left)
            Rocket.LeftMotor();
        if (e.Key == Key.Right)
            Rocket.RightMotor();
        OnPropertyChanged(nameof(RocketPositionY));
    }

    public double RocketPositionY => Rocket.PositionY;
    public double RocketPositionX => Rocket.PositionX;
}