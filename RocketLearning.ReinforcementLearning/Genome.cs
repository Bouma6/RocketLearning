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
    // Creates a new clone of a genome 
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
    // Mutates the genomes Connections, Nodes and Weights 
    //If the program will be too slow used the mutation tricks to make it faster
    public void Mutate(Config config, Random random)
    {
        //1 - Mutate weights of connections 
        MutateWeights(config, random);
        
        //2 - Mutate Nodes 
        //Add nodes
        if (random.NextDouble() < config.AddNodeRate)
        {
            MutateNodesAdd(config, random);
        }
        //Remove nodes    
        if (random.NextDouble() < config.RemoveNodeRate)
        {
            MutateNodesRemove(config, random);
        }
        
        //3 - Mutate Connections
        //Add connections
        if (random.NextDouble() < config.AddConnectionRate)
        {
            MutateConnectionsAdd(config, random);    
        }
        //RemoveConnections
        if (random.NextDouble() < config.RemoveConnectionRate)
        {
            MutateConnectionsRemove(random);    
        }
    }
    
    private void MutateWeights(Config config, Random random)
    {
        foreach (var conection in Connections)
        {
            if (random.NextDouble() < config.WeightMutateRate)
            {
                //Replace weight by a completely new number 
                if (random.NextDouble() < config.WeightChangeProbability)
                {
                    conection.Weight = RandomWeight(random);
                }
                //Change the weight by a small number 
                else
                {
                    double deltaWeight = GaussianRandom(random) * config.WeightMutatePower;
                    conection.Weight = Clamp(conection.Weight + deltaWeight, Config.MaxWeight,Config.MinWeight);
                }
            }

            if (random.NextDouble() < Config.EnableConnectionRate)
            {
                conection.Active = !conection.Active;   
            }
        }
    }

    private static double RandomWeight(Random random)
    {
        return random.NextDouble() * (Config.MaxWeight - Config.MinWeight) + Config.MinWeight;
    }

    private static double GaussianRandom(Random random)
    {
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();
        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
    }
    //Clamp value in between min and max 
    private static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0) return min;
        if (value.CompareTo(max) > 0) return max;
        return value;
    }
    private void MutateConnectionsAdd(Config config, Random random)
    {
        var nodeA = Nodes[random.Next(Nodes.Count)];
        var nodeB = Nodes[random.Next(Nodes.Count)];
        
        //Do not allow self connections or duplicate connections
        if (nodeA.Id == nodeB.Id || Connections.Any(c => c.FromId == nodeA.Id && c.ToId == nodeB.Id))
            return;
        
        var newConnection = new Connections
        {
            FromId = nodeA.Id,
            ToId = nodeB.Id,
            Weight = RandomWeight(random),
            InnovationNumber = InnovationTracker.GetInnovationNumber(nodeA.Id, nodeB.Id),
            Active = true
        };
        Connections.Add(newConnection);
    }
    private void MutateConnectionsRemove(Random random)
    {
        if (Connections.Count == Config.MinimumConnections) return;
        Connections.Remove(Connections[random.Next(Connections.Count)]);
    }
    private void MutateNodesAdd(Config config, Random random)
    {
        
    }
    private void MutateNodesRemove(Config config, Random random)
    {
        
    }
    
}