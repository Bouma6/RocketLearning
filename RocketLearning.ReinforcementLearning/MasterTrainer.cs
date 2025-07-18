using System.Text.Json;

namespace RocketLearning.ReinforcementLearning;

public class MasterTrainer
{
    private readonly Random _random = new();
    private readonly Config _config = new();
    private readonly List<Trainer> _trainers = [];
    private readonly FitnessEvaluatorDelegate _evaluator;
    private readonly int _cores;
    private readonly int _perCore;
    private const string SavedGenomePath = "RocketLearningNN_lastest.json";

    private Genome BestGenome { get; set; } = new Genome{Fitness = double.NegativeInfinity};
    
    public NeuralNetwork BestNetwork => BestGenome?.BuildNeuralNetwork(Config.Activation);

    public MasterTrainer(FitnessEvaluatorDelegate fitnessEvaluator)
    {
        _evaluator = fitnessEvaluator;

        _cores = _config.Cores;
        _perCore = Config.PopulationSize / _cores;

        InitializePopulation(Config.PopulationSize);
    }

    public void Run(Action<double>? reportProgress = null, int generation = Config.NumberOfIterations)
    {
        int syncEvery = Config.SynchronizationLength;
        int syncSteps = generation / syncEvery;

        for (int i = 0; i < syncSteps; i++)
        {
            Parallel.ForEach(_trainers, trainer =>
            {
                trainer.RunGenerations(syncEvery);
            });

            UpdateBestGenome();
            SynchronizePopulations();

            // Report progress back to UI
            reportProgress?.Invoke((i + 1) / (double)syncSteps);
        }
        SaveBestGenome();
    }


    private void SaveBestGenome()
    {
        if (double.IsNegativeInfinity(BestGenome.Fitness)) return;
        var json = JsonSerializer.Serialize(BestGenome);
        File.WriteAllText(SavedGenomePath, json);
        Console.WriteLine("Best genome saved");
        
    }
    public void LoadBestGenome()
    {
        if (!File.Exists(SavedGenomePath)) return;
        Console.WriteLine("Loading best genome");
        var json = File.ReadAllText(SavedGenomePath);
        BestGenome = JsonSerializer.Deserialize<Genome>(json)!;
    }
    private void UpdateBestGenome()
    {
        var best = _trainers
            .SelectMany(t => t.Population)
            .Where(g => g.Fitness !=0)
            .OrderByDescending(g => g.Fitness)
            .FirstOrDefault();
        Console.WriteLine($"Best score of this generation {best?.Fitness:F2}");
        if (best != null && (best.Fitness > BestGenome.Fitness))
        {
            BestGenome = best.Clone();
        }
    }

    private void SynchronizePopulations()
    {
        var allGenomes = _trainers.SelectMany(t => t.Population).ToList();

        var selected = Config.Selection(allGenomes, allGenomes.Count, _random);

        var chunks = selected
            .Select((g, i) => new { g, i })
            .GroupBy(x => x.i / _perCore)
            .Select(g => g.Select(x => x.g.Clone()).ToList())
            .ToList();

        for (int i = 0; i < _trainers.Count; i++)
        {
            _trainers[i].Population = (i < chunks.Count) ? chunks[i] : new();
        }
    }

    private void InitializePopulation(int genomeCount)
    {
        var population = new List<Genome>();

        for (int i = 0; i < genomeCount; i++)
        {
            var newGenome = new Genome();

            // Inputs
            for (int j = 0; j < Config.InputSize; j++)
            {
                newGenome.Nodes.Add(new Node
                {
                    Id = j,
                    Type = NodeType.Input,
                });
            }

            // Bias
            newGenome.Nodes.Add(new Node
            {
                Id = Config.BiasNodeId,
                Type = NodeType.Bias,
            });

            // Outputs
            for (int j = 0; j < Config.OutputSize; j++)
            {
                newGenome.Nodes.Add(new Node
                {
                    Id = Config.OutputNodesId + j,
                    Type = NodeType.Output,
                });
            }

            // Random initial connections
            for (int j = 0; j < Config.InputSize; j++)
            {
                for (int k = 0; k < Config.OutputSize; k++)
                {
                    if (Random.Shared.NextDouble() < Config.StartingConnectionsRate)
                    {
                        newGenome.Connections.Add(new Connections
                        {
                            FromId = j,
                            ToId = k + Config.OutputNodesId,
                            Weight = Random.Shared.NextDouble() * 2 - 1,
                            Active = true,
                            InnovationNumber = InnovationTracker.GetInnovationNumber(j, k + Config.OutputNodesId)
                        });
                    }
                }
            }

            population.Add(newGenome);
        }

        for (int i = 0; i < _cores; i++)
        {
            var slice = population
                .Skip(i * _perCore)
                .Take(_perCore)
                .Select(g => g.Clone())
                .ToList();

            _trainers.Add(new Trainer(slice, _evaluator, _config));
        }
    }
}




