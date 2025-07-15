using Avalonia.Input;
using RocketLearning.Game;

namespace RocketLearning.Agent;

public class HumanAgent : IAgent
{
    private Key? _lastKey;

    public void OnKeyPressed(KeyEventArgs e)
    {
        _lastKey = e.Key;
    }

    public RocketInput Decide(GameState state)
    {
        var help = _lastKey;
        _lastKey = null;
        switch (help)
        {
            case Key.Left:
                return RocketInput.Left;
            case Key.Right:
                return RocketInput.Right;
            case Key.Up:
                return RocketInput.Up;
            default:
                return RocketInput.None;
        }
    }
}