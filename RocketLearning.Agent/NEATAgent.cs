using RocketLearning.Game;
using RocketLearning.ReinforcementLearning;

namespace RocketLearning.Agent;

public class NEATAgent : IAgent
{
    private readonly NeuralNetwork _network;

    public NEATAgent(NeuralNetwork network)
    {
        _network = network;
    }

    public RocketInput Decide(GameState state)
    {
        var inputs = new[]
        {
            state.Rocket.PositionX,
            state.Rocket.PositionY,
            state.Rocket.Angle,
            state.Rocket.VelocityX,
            state.Rocket.VelocityY,
            state.Terrain.PlatformX,
            state.Terrain.PlatformY
        };

        //var outputs = _network.FeedForward(inputs);

        return RocketInput.None;
    }
}