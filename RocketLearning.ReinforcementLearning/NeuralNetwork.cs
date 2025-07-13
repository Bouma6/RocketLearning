//Test it. 
//CrossOver improvements 

namespace RocketLearning.ReinforcementLearning;
using System;

public class NeuralNetwork(ActivationDelegate activation)
{
    public List<Node> Nodes = [];
    public List<Connections> Connections = [];

    public double[] FeedForward(double[] inputValues)
    {
        
        // Validate expected output nodes
        int outputCount = Nodes.Count(n => n.Type == NodeType.Output);
        if (outputCount != Config.OutputSize)
        {
            throw new InvalidOperationException(
                $"[FeedForward] Expected {Config.OutputSize} output nodes, but found {outputCount}.\n" +
                $"Node IDs: {string.Join(", ", Nodes.Select(n => $"(ID: {n.Id}, Type: {n.Type})"))}"
            );
        }

        var nodeMap = Nodes.ToDictionary(n => n.Id);
        int inputIndex = 0;
        foreach (Node node in Nodes)
        {
            if (node.Type == NodeType.Input)
            {
                node.Weight = inputValues[inputIndex++];
            }
            else if (node.Type == NodeType.Bias)
            {
                node.Weight = 1;
            }
            else
            {
                node.Weight = 0;
            }
        }

        var sorted = TopologicalSort();
        foreach (var node in sorted)
        {
            foreach (var connection in Connections)
            {
                if (connection.Active && connection.FromId ==node.Id)
                {
                    //var target = Nodes.First(n => n.Id == connection.ToId);
                    var target = nodeMap[connection.ToId];
                    target.Weight += node.Weight* connection.Weight;
                }
            }
        }
        var outputValues = Nodes
            .Where(n => n.Type == NodeType.Output)
            .Select(n => activation(n.Weight))
            .ToArray();
        if (outputValues.Length != Config.OutputSize)
            throw new InvalidOperationException($"Expected {Config.OutputSize} outputs but got {outputValues.Length}");

        return outputValues;
    }


    private List<Node> TopologicalSort()
    {
        var nodeMap = Nodes.ToDictionary(n => n.Id);
        var incomingEdges = Nodes.ToDictionary(n => n.Id, _ => 0);

        foreach (var connection in Connections)
        {
            if (connection.Active)
                incomingEdges[connection.ToId]++;
        }

        var queue = new Queue<Node>(
            Nodes.Where(n => incomingEdges[n.Id] == 0)
        );

        var sorted = new List<Node>();
        var visited = new HashSet<int>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sorted.Add(current);
            visited.Add(current.Id);

            foreach (var conn in Connections)
            {
                if (!conn.Active || conn.FromId != current.Id) continue;

                int toId = conn.ToId;
                incomingEdges[toId]--;

                if (incomingEdges[toId] == 0 && !visited.Contains(toId))
                {
                    queue.Enqueue(nodeMap[toId]);
                }
            }
        }
        // Some invalid nodes are still getting through 
        // Try to make it such that no cyclic or unreached nodes even get in here 
        if (sorted.Count != Nodes.Count)
        {
            //Console.WriteLine("There are some nodes that are either unreachable or not in the graph.");
        }
        
        return sorted;
    }
}
