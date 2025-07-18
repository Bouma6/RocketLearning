namespace RocketLearning.ReinforcementLearning;

public class Genome
{
    public List<Node> Nodes = [];
    public List<Connections> Connections = [];
    public double Fitness { get; set; }
    
    
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
            MutateNodesAdd(random);
        }
        //Remove nodes    
        if (random.NextDouble() < config.RemoveNodeRate)
        {
            MutateNodesRemove(random);
        }
        
        //3 - Mutate Connections
        //Add connections
        if (random.NextDouble() < config.AddConnectionRate)
        {
            MutateConnectionsAdd(random);    
        }
        //Deactivate Connections
        if (random.NextDouble() < config.RemoveConnectionRate)
        {
            MutateConnectionsRemove(random);    
        }
        //Reactivate
        if (random.NextDouble()<config.ReactivateRate)
        {
            MutateConnectionsReactivate(random);
        }
    }
    
    private void MutateWeights(Config config, Random random)
    {
        foreach (var connection in Connections)
        {
            if (random.NextDouble() < config.WeightMutateRate)
            {
                //Replace weight by a completely new number 
                if (random.NextDouble() < config.WeightChangeProbability)
                {
                    connection.Weight = RandomWeight(random);
                }
                //Change the weight by a small number 
                else
                {
                    double deltaWeight = GaussianRandom(random) * config.WeightMutatePower;
                    connection.Weight = Clamp(connection.Weight + deltaWeight, Config.MaxWeight,Config.MinWeight);
                }
            }

            if (random.NextDouble() < Config.EnableConnectionRate)
            {
                connection.Active = !connection.Active;   
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
    private void MutateConnectionsReactivate(Random random)
    {
        // Only attempt if there are inactive connections
        var inactiveConnections = Connections.Where(c => !c.Active).ToList();
        if (inactiveConnections.Count == 0) return;

        var toReactivate = inactiveConnections[random.Next(inactiveConnections.Count)];
        toReactivate.Active = true;
        
    }

    private void MutateConnectionsAdd(Random random)
    {
        const int maxAttempts = 4;

        var fromCandidates = Nodes.Where(n =>
            n.Type == NodeType.Input || n.Type == NodeType.Hidden || n.Type == NodeType.Bias).ToList();

        var toCandidates = Nodes.Where(n =>
            n.Type == NodeType.Hidden || n.Type == NodeType.Output).ToList();

        if (fromCandidates.Count == 0 || toCandidates.Count == 0)
            return;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var from = fromCandidates[random.Next(fromCandidates.Count)];
            var to = toCandidates[random.Next(toCandidates.Count)];

            if (from.Id == to.Id || Connections.Any(c => c.FromId == from.Id && c.ToId == to.Id))
                continue;
            // Enforce direction rule from.Id < to.Id or to is output -just trying 
            if (from.Id >= to.Id && to.Type != NodeType.Output)
                continue;
            Connections.Add(new Connections
            {
                FromId = from.Id,
                ToId = to.Id,
                Weight = RandomWeight(random),
                InnovationNumber = InnovationTracker.GetInnovationNumber(from.Id, to.Id),
                Active = true
            });

            return;
        }
    }

    private void MutateConnectionsRemove(Random random)
    {
        // Only attempt if there are any active connections
        var activeConnections = Connections.Where(c => c.Active).ToList();
        if (activeConnections.Count == 0) return;

        // Randomly deactivate one connection
        var toDeactivate = activeConnections[random.Next(activeConnections.Count)];
        toDeactivate.Active = false;
    }
    private void MutateNodesAdd(Random random)
    {
        // Find a connection we can split and add a node in middle of 
        var availableNodes = Connections.Where(c=>c.Active).ToList();
        if (availableNodes.Count == 0) return;
        
        var connection = availableNodes[random.Next(availableNodes.Count)];
        connection.Active = false;
        
        // Create a node with new NodeId
        int newNodeId = NodeIdGenerator.GetNextNodeId();
        Nodes.Add( new Node
        {
            Id = newNodeId,
            Type = NodeType.Hidden
        });

        var newConnection1 = new Connections
        {
            FromId = connection.FromId,
            ToId = newNodeId,
            Weight = 1,
            Active = true,
            InnovationNumber = InnovationTracker.GetInnovationNumber(connection.FromId, newNodeId)
        };
        
        var newConnection2 = new Connections
        {
            FromId = newNodeId,
            ToId = connection.ToId,
            //Do not lose the information about the weight
            Weight = connection.Weight,
            Active = true,
            InnovationNumber = InnovationTracker.GetInnovationNumber(newNodeId, connection.ToId)
        };
        Connections.Add(newConnection1);
        Connections.Add(newConnection2);
    }
    private void MutateNodesRemove(Random random)
    {
        //Remove only a hidden node - Removing input , output or bias node would break the NN
        var availableNodes = Nodes.Where(n =>n.Type == NodeType.Hidden).ToList();
        if (availableNodes.Count == 0) return;
        
        //Pick one and remove it 
        var nodeToRemove = availableNodes[random.Next(availableNodes.Count)];
        Nodes.Remove(nodeToRemove);
        
        //Remove all the connections going to and from the node
        Connections.RemoveAll(c=>c.FromId == nodeToRemove.Id||c.ToId == nodeToRemove.Id);
    }

    public static Genome Crossover(Genome parent1, Genome parent2, Random random)
    {
        var (fitter, weaker) = parent1.Fitness > parent2.Fitness
            ? (parent1, parent2)
            : (parent2, parent1);

        var newChild = new Genome();
        var weakConnections = weaker.Connections.ToDictionary(c => c.InnovationNumber);

        // 1. Inherit connections
        foreach (var gene in fitter.Connections)
        {
            Connections childGene;
            if (weakConnections.TryGetValue(gene.InnovationNumber, out var matchingGene))
            {
                childGene = random.NextDouble() < 0.5 ? CloneConnection(gene) : CloneConnection(matchingGene);

                if (!gene.Active || !matchingGene.Active)
                    childGene.Active = !(random.NextDouble() < 0.75);
            }
            else
            {
                childGene = CloneConnection(gene);
            }

            newChild.Connections.Add(childGene);
        }

        // 2. Add input, output, and bias nodes exactly once
        var allNodes = parent1.Nodes.Concat(parent2.Nodes).DistinctBy(n => n.Id).ToList();
        foreach (var node in allNodes)
        {
            if (node.Type == NodeType.Input || node.Type == NodeType.Output || node.Type == NodeType.Bias)
            {
                newChild.Nodes.Add(new Node
                {
                    Id = node.Id,
                    Type = node.Type,
                    Weight = 0
                });
            }
        }

        // 3. Add used hidden nodes only
        var usedNodeIds = newChild.Connections
            .SelectMany(c => new[] { c.FromId, c.ToId })
            .ToHashSet();

        foreach (var node in allNodes)
        {
            if (node.Type == NodeType.Hidden && usedNodeIds.Contains(node.Id))
            {
                newChild.Nodes.Add(new Node
                {
                    Id = node.Id,
                    Type = node.Type,
                    Weight = 0
                });
            }
        }

        return newChild;
    }
    private static Connections CloneConnection(Connections original)
    {
        return new Connections
        {
            FromId = original.FromId,
            ToId = original.ToId,
            Weight = original.Weight,
            InnovationNumber = original.InnovationNumber,
            Active = original.Active
        };
    }

    public double DistanceTo(Genome other)
    {
        var thisGenes = Connections.ToDictionary(c => c.InnovationNumber);
        var otherGenes = other.Connections.ToDictionary(c => c.InnovationNumber);
        
        // Combine all innovation numbers from both genomes
        var allInnovations = new HashSet<int>(thisGenes.Keys);
        allInnovations.UnionWith(otherGenes.Keys);

        int matching = 0;
        double weightDiffSum = 0;

        int disjoint = 0;
        int excess = 0;

        int maxInnov1 = thisGenes.Keys.Count > 0 ? thisGenes.Keys.Max() : -1;
        int maxInnov2 = otherGenes.Keys.Count > 0 ? otherGenes.Keys.Max() : -1;

        foreach (var innov in allInnovations)
        {
            bool in1 = thisGenes.TryGetValue(innov, out var gene1);
            bool in2 = otherGenes.TryGetValue(innov, out var gene2);

            if (in1 && in2)
            {
                matching++;
                weightDiffSum += Math.Abs(gene1!.Weight - gene2!.Weight);
            }
            else
            {
                // Check for excess genes
                bool isExcess = (in1 && innov > maxInnov2) || (in2 && innov > maxInnov1);
                if (isExcess)
                    excess++;
                else
                    disjoint++;
            }
        }
        
        // Normalize by genome size to avoid penalizing small genomes too harshly
        int n = Math.Max(thisGenes.Count, otherGenes.Count);
        if (n < 20) n = 1;
        
        
        // Calculate average weight difference of matching genes

        double avgWeightDiff = matching > 0 ? weightDiffSum / matching : 0.0;
        
        // Compute genetic distance using NEAT formula:
        return (Config.ExcessGeneCoefficient * excess / n) + (Config.DisjointGeneCoefficient * disjoint / n) + (Config.AverageWeightDifferenceCoefficient * avgWeightDiff);
    }


}  