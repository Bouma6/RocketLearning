using RocketLearning.Game;
using RocketLearning.ReinforcementLearning;

namespace RocketLearning.Agent;

public static class GameEvaluator
{
    public static double EvaluateNetwork(NeuralNetwork network, int maxTicks = 10_000, double deltaTime = 1f / 60)
    {
        var agent = new NeatAgent(network);
        var game = new GameState();

        for (int i = 0; i < maxTicks && !game.GameOver; i++)
        {
            var input = agent.Decide(game);
            game.Tick(input, deltaTime);
        }

        return game.Score;
    }
}