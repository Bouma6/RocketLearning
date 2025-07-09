//Create a clean NeuralNetwork class (no game logic)
//Add a FeedForward method & test it
//Add a Genome class (basic structure)
//Use network in NEATAgent
//Add Trainer to evolve networks


namespace RocketLearning.ReinforcementLearning;
using System;
public class NeuralNetwork(Func<double, double> activation)
{
    
    public List<Node> Nodes = [];
    public List<Connections> Connections = [];
    public Func<double, double> ActivationFunction = activation;

    public double[] FeedForward(double[] inputValues)
    {
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
                    target.Weight += ActivationFunction(node.Weight* connection.Weight);
                }
            }
        }
        return Nodes
            .Where(n => n.Type == NodeType.Output)
            .Select(n => ActivationFunction(n.Weight))
            .ToArray();
    }


    private List<Node> TopologicalSort()
    {
        //TO DO: do proper implementation when I will be less tired 
        return Nodes.OrderBy(n => n.Type).ToList();
    }
    
}
