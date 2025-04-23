using System.Windows.Input;
using Avalonia.Input;

namespace RocketLearning.ViewModels;

public class GameViewModel : ViewModelBase
{
    private int _rocketY;
    public int RocketY
    {
        get => _rocketY;
        set => SetProperty(ref _rocketY, value);
    }

    public GameViewModel()
    {
        RocketY = 100;
    }

    public void OnKeyPressed(KeyEventArgs e)
    {
        if (e.Key == Key.Up) RocketY -= 10;
        if (e.Key == Key.Down) RocketY += 10;
    }
}