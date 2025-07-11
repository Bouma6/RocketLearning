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
        //Get fitness of each genome
        foreach (var genome in Population)
        {
            var network = genome.BuildNeuralNetwork(Config.Activation);
            genome.Fitness = _evaluate(network);
        }
        
        //Elitism - pick _config.Elitism the best individuals that will automatically survive to the next generation
        //This way we never lose the best
        var elites = Population
            .OrderByDescending(g => g.Fitness)
            .Take(_config.Elitism)
            .Select(g =>g.Clone())
            .ToList();
        
        //Select the rest of the population using Selector
        var remaining = Config.PopulationSize-_config.Elitism;
        var selected = Selector(Population,remaining,_random);
        
        //Clone and mutate the selected offsprings
        var offspring = selected.Select(parent =>
        {
            var child = parent.Clone();
            child.Mutate(_config, _random);
            return child;
        }).ToList();
        
        //CrossOver of the offsprings
        //Overload it, so I can call it on list in Genome after done with mutation 
        //var crossover =offspring.CrossOver(_config, _random);
        
        
        Population = elites.Concat(offspring).ToList();
        Generation++;
    }
    
    public void RunGenerations(int count =Config.SynchronizationLength){
        for (int i = 0; i < count; i++)
        {
            RunGeneration();
        }
    }
}