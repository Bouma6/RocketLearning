using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLearning.ReinforcementLearning;

public class MasterTrainer
{
    private readonly Random _random = new();
    private readonly Config _config = new();
    private readonly List<Trainer> _trainers = [];
    private readonly FitnessEvaluatorDelegate _evaluator;
    private readonly int _cores;
    private readonly int _perCore;

    public Genome BestGenome { get; private set; } = new();
    public NeuralNetwork BestNetwork => BestGenome?.BuildNeuralNetwork(Config.Activation);

    public MasterTrainer(FitnessEvaluatorDelegate fitnessEvaluator)
    {
        _evaluator = fitnessEvaluator;

        _cores = _config.Cores;
        _perCore = Config.PopulationSize / _cores;

        InitializePopulation(Config.PopulationSize);
    }

    public void Run(int generation = Config.NumberOfIterations)
    {
        int syncEvery = Config.SynchronizationLength;
        int syncSteps = generation / syncEvery;

        Console.WriteLine($"Starting training for {generation} generations in {syncSteps} sync steps...");

        for (int i = 0; i < syncSteps; i++)
        {
            Console.WriteLine($"Running sync step {i + 1}/{syncSteps}");

            // Run each trainer in parallel
            var tasks = _trainers.Select(trainer =>
                Task.Run(() => trainer.RunGenerations(syncEvery))
            ).ToArray();

            Task.WaitAll(tasks);

            // Update best genome & synchronize population
            UpdateBestGenome();
            SynchronizePopulations();

            Console.WriteLine($"Best score so far: {BestGenome?.Fitness:F2}");
        }
    }

    private void UpdateBestGenome()
    {
        var best = _trainers
            .SelectMany(t => t.Population)
            .OrderByDescending(g => g.Fitness)
            .FirstOrDefault();

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
                    if (Random.Shared.NextDouble() < Config.StartingConectionsRate)
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
