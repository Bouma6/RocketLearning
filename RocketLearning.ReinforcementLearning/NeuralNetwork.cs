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
                $"Expected {Config.OutputSize} output nodes, but found {outputCount}.\n" +
                $"Node IDs: {string.Join(", ", Nodes.Select(n => $"(ID: {n.Id}, Type: {n.Type})"))}"
            );
        }

        var nodeMap = Nodes.ToDictionary(n => n.Id);
        var inputIndex = 0;
        
        foreach (Node node in Nodes)
        {
            switch (node.Type)
            {
                case NodeType.Input:
                    node.Weight = inputValues[inputIndex++];
                    break;
                case NodeType.Bias:
                    node.Weight = 1;
                    break;
                default:
                    node.Weight = 0;
                    break;
            }
        }

        var sorted = TopologicalSort();
        //Knowing that the graph is topologically sorted we can go straight through it and calculate the weights of all the nodes 
        foreach (var node in sorted)
        {
            foreach (var connection in Connections)
            {
                if (connection.Active && connection.FromId ==node.Id)
                {
                    var target = nodeMap[connection.ToId];
                    target.Weight += node.Weight* connection.Weight;
                }
            }
        }
        //Keep only output Nodes
        var outputValues = Nodes
            .Where(n => n.Type == NodeType.Output)
            .Select(n => activation(n.Weight))
            .ToArray();
        return outputValues;
    }

    //Make the graph topologically sorted and oriented such that we do not encounter any circular references 
    private List<Node> TopologicalSort()
    {
        var nodeMap = Nodes.ToDictionary(n => n.Id);
        var incomingEdges = Nodes.ToDictionary(n => n.Id, _ => 0);

        foreach (var connection in Connections.Where(connection => connection.Active))
        {
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

                var toId = conn.ToId;
                incomingEdges[toId]--;

                if (incomingEdges[toId] == 0 && !visited.Contains(toId))
                {
                    queue.Enqueue(nodeMap[toId]);
                }
            }
        }
        return sorted;
    }
}
