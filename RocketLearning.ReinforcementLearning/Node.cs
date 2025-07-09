namespace RocketLearning.ReinforcementLearning;

public class Node
{
    public int Id { get; set; }
    public double Weight { get; set; } = 0.0;
    public NodeType Type { get; set; }
}