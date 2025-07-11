using System.Collections.Concurrent;

namespace RocketLearning.ReinforcementLearning;
// Thread safe Innovation number tracker !for NEAT!
public static class InnovationTracker
{
    private static readonly ConcurrentDictionary<(int fromId, int toId), int> ConnectionHistory = new();
    private static int _currentInnovation = 0;

    public static int GetInnovationNumber(int fromId, int toId)
    {
        var key = (fromId, toId);
        return ConnectionHistory.GetOrAdd(key, _ =>
            Interlocked.Increment(ref _currentInnovation));
    }
}