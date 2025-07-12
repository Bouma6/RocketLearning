namespace RocketLearning.ReinforcementLearning;

public class MasterTrainer
{
    private readonly Random _random = new Random();
    private readonly Config _config = new Config();

    private readonly List<Trainer> _trainers = [];
    private readonly int _cores;
    private readonly int _perCore;
    private readonly FitnessEvaluatorDelegate _evaluator;
    private Genome? BestGenome { get; set; }
    public NeuralNetwork BestNetwork => BestGenome?.BuildNeuralNetwork(Config.Activation)!;


    public MasterTrainer(FitnessEvaluatorDelegate fitnessEvaluator)
    {
        InitializePopulation(Config.PopulationSize);
        
        _cores = _config.Cores;
        _perCore = Config.PopulationSize / _cores;
        _evaluator = fitnessEvaluator;
    }

    public void Run(int generation = Config.NumberOfIterations)
    {
        int syncEvery = Config.SynchronizationLength;
        int syncSteps = generation / syncEvery;
        Console.WriteLine("zaciname");
        for (int i = 0; i < syncSteps; i++)
        {
            Console.WriteLine("je to pici");
            Parallel.ForEach(_trainers, trainer =>
            {
                trainer.RunGenerations(syncEvery);
            });
            UpdateBestGenome();
            SynchronizePopulations();
        }
    }

    private void SynchronizePopulations()
    {
        var allGenomes = _trainers.SelectMany(t => t.Population).ToList();
        var newGenomes = Config.Selection(allGenomes,allGenomes.Count,_random);
        
        for (int i = 0; i < _trainers.Count; i++)
        {
            var slice = newGenomes
                .Skip(i * _perCore)
                .Take(_perCore)
                .Select(g => g.Clone())
                .ToList();

            _trainers[i].Population = slice;
        }
        
    }
    private void InitializePopulation(int genomeCount)
    {
        var population = new List<Genome>();

        for (int i = 0; i < genomeCount; i++)
        {
            var newGenome = new Genome();
            //Input Nodes 
            for (int j = 0; j < Config.InputSize; j++)
            {
                newGenome.Nodes.Add(new Node
                {
                    Id = j,
                    Type = NodeType.Input,
                });
            }
            //Bias Node 
            newGenome.Nodes.Add(new Node
            {
                Id = Config.BiasNodeId,
                Type = NodeType.Bias,
            });
            
            //Output Nodes
            for (int j = 0; j < Config.OutputSize; j++)
            {
                newGenome.Nodes.Add(new Node
                {
                    Id = Config.OutputNodesId +j,
                    Type = NodeType.Output,
                });
            }
            // Create a Few Random edges 
            for (var j = 0; j < Config.InputSize; j++)
            {
                for (var k = 0; k < Config.OutputSize; k++)
                {
                    if (Random.Shared.NextDouble() < Config.StartingConectionsRate)
                    {
                        newGenome.Connections.Add(new Connections
                        {
                            FromId = j,
                            ToId = k+Config.OutputNodesId,
                            // random weight -1 - 1
                            Weight = Random.Shared.NextDouble() * 2 - 1,
                            Active = true,
                            InnovationNumber = InnovationTracker.GetInnovationNumber(j, k+Config.OutputNodesId)
                        });
                    }
                }
            }
            population.Add(newGenome);
        }
        for (int i = 0; i < _cores;i++)
        {
            var currentPopulation = population
                .Skip(i * _perCore)
                .Take(_perCore)
                .Select(g =>g.Clone())
                .ToList();
            var trainer = new Trainer(currentPopulation, _evaluator, _config);
            _trainers.Add(trainer);
        }
    }
    private void UpdateBestGenome()
    {
        var best = _trainers
            .SelectMany(t => t.Population)
            .OrderByDescending(g => g.Fitness)
            .FirstOrDefault();

        if (best != null && (BestGenome == null || best.Fitness > BestGenome.Fitness))
        {
            BestGenome = best.Clone();
        }
    }
    
}