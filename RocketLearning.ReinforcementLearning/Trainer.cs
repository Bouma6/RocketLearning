using System;

namespace RocketLearning.ReinforcementLearning;
public delegate double FitnessEvaluatorDelegate(NeuralNetwork network);

public class Trainer
{
    public List<Genome> Population { get; set; }
    
    private readonly Random _random;
    public int Generation { get; private set; } = 0;
    
    private Config _config;

    private readonly FitnessEvaluatorDelegate _evaluate;
    public SelectionDelegate Selector= Config.Selection;
    public Trainer(List<Genome> originalPopulation, FitnessEvaluatorDelegate evaluate,Config config ,int seed = 42)
    {
        _config = config;
        _evaluate = evaluate;
        _random = new Random(seed);
        Population = originalPopulation;
    }

    private void RunGeneration()
    {
        Console.WriteLine($"Generating: {Generation} ");
        //1. Get fitness of each genome
        foreach (var genome in Population)
        {
            var network = genome.BuildNeuralNetwork(Config.Activation);
            genome.Fitness = _evaluate(network);
        }
        
        //2. Elitism - pick _config.Elitism the best individuals that will automatically survive to the next generation
        //This way we never lose the best
        var elites = Population
            .OrderByDescending(g => g.Fitness)
            .Take(_config.Elitism)
            .Select(g =>g.Clone())
            .ToList();
        
        //3. Select the rest of the population using Selector
        var remaining = Config.PopulationSize-_config.Elitism;
        var selected = Selector(Population,remaining,_random);
        
        //4. CrossOver and Mutation
        //Later on speciation of crossover 
        List<Genome> offspring = [];
        foreach (var child1 in selected)
        {
            /*
            Genome newGenome;
            var child2 = selected[_random.Next(selected.Count)];
            if (_random.NextDouble() < _config.CrossOverRate && !ReferenceEquals(child1, child2))
            {
                newGenome = Genome.Crossover(child1, child2, _random);
            }
            else
            {
                newGenome = child1.Clone();
            }
            */
            var newGenome = child1.Clone();
            newGenome.Mutate(_config,_random);
            offspring.Add(newGenome);
            

        }
        
        Population = elites.Concat(offspring).ToList();
        Generation++;
    }
    
    public void RunGenerations(int count =Config.SynchronizationLength){
        Console.WriteLine("We are in a trainer");
        for (int i = 0; i < count; i++)
        {
            RunGeneration();
        }
    }
}