using RocketLearning.Game;
using RocketLearning.ReinforcementLearning;

namespace RocketLearning.Agent;

public static class GameEvaluator
{
    public static double EvaluateNetwork(NeuralNetwork network, int maxTicks = 10_000, double deltaTime = 1f / 60)
    {
        //Spawn values at which the NN will be evaluated
        double[] spawnPoints = [200, 500, 800, 1100, 1400];
        double finalScore = 0;
        foreach (var spawnPoint in spawnPoints)
        {
            var agent = new NeatAgent(network);
            var game = new GameState(spawnPoint);

            for (var i = 0; i < maxTicks && !game.GameOver; i++)
            {
                var input = agent.Decide(game);
                game.Tick(input, deltaTime);
            }

            finalScore += game.Score;
        }
        return finalScore /spawnPoints.Length;
    }
}