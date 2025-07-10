namespace RocketLearning.ReinforcementLearning;

public class Genome
{
    public List<Node> Nodes = [];
    public List<Connections> Connections = [];
    public double Fitness = 0;
    
    
    //Build Neural Network from the genome, nodes and connections are copied 
    public NeuralNetwork BuildNeuralNetwork(Func<double, double> activation)
    {
        var network = new NeuralNetwork(activation)
        {
            Nodes = Nodes.Select(n => new Node
            {
                Weight = 0,
                Id = n.Id,
                Type = n.Type
            }).ToList(),

            Connections = Connections.Select(c => new Connections
            {
                ToId = c.ToId,
                FromId = c.FromId,
                Weight = c.Weight,
                InnovationNumber = c.InnovationNumber,
                Active = c.Active
            }).ToList()
        };
        return network;
    }
}