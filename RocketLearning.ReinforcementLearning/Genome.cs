namespace RocketLearning.ReinforcementLearning;

public class Genome
{
    public List<Node> Nodes = [];
    public List<Connections> Connections = [];
    public double Fitness = 0;
    
    
    //Build Neural Network from the genome, nodes and connections are copied 
    public NeuralNetwork BuildNeuralNetwork(ActivationDelegate activation)
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

    public Genome Clone()
    {
        var clone = new Genome()
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
            }).ToList(),
            
            Fitness = Fitness
        };
        return clone;
    }

    public void Mutate(Config config, Random random)
    {
        
    }


    public List<Genome> CrossOver(Config config, Random random)
    {
        
    }
}