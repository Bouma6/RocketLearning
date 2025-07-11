using System.Threading;

namespace RocketLearning.ReinforcementLearning;

public static class NodeIdGenerator
{
    //IDs under 10 000 are reserved for special use cases - use them as you wish 
    private static int _currentId = 10_000; 

    public static int GetNextNodeId()
    {
        return Interlocked.Increment(ref _currentId);
    }
}