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
        

        var outputs = _network.FeedForward(inputs);
        var best = 0;
        // safety measure 
        if (outputs.Length != 3)
            throw new InvalidOperationException("Expected exactly 3 output nodes.");
        
        for (var i = 0; i < outputs.Length; ++i)
        {
            if (outputs[i] > outputs[best])
            {
                best = i;
            }
        }
        return best switch
        {
            0 => RocketInput.Left,
            1 => RocketInput.Right,
            _ => RocketInput.None
        };
    }
}