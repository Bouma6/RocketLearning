using RocketLearning.Game;
using RocketLearning.ReinforcementLearning;

namespace RocketLearning.Agent;

public class NeatAgent(NeuralNetwork network) : IAgent
{
    public RocketInput Decide(GameState state)
    {
        // When adding more inputs do not forget to also adjust in the config file numberOfInputs
        var inputs = new[]
        {
            (state.Rocket.PositionX-800)/800,
            (state.Rocket.PositionY-450)/450,
            (state.Rocket.Angle-9)/9,
            (state.Rocket.VelocityX-200)/200,
            (state.Rocket.VelocityY-200)/200,
        };
        
        // Let the NN chose the best action to take
        var outputs = network.FeedForward(inputs);
        
        // safety measure 
        if (outputs.Length != 4)
            throw new InvalidOperationException($"Expected exactly 3 output nodes but got {outputs.Length}.");
        var best = 0;
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
            2 => RocketInput.Up,
            _ => RocketInput.None
        };
    }
}