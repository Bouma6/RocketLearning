namespace RocketLearning.Agent;
using RocketLearning.Game;
public interface IAgent
{
    RocketInput Decide(GameState state);
}