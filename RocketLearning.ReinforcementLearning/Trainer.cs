namespace RocketLearning.ReinforcementLearning;
public delegate double FitnessEvaluatorDelegate(NeuralNetwork network);

public class Trainer
{
    public List<Genome> Population { get; set; }
    
    private readonly Random _random;
    private int Generation { get; set; }
    
    private Config _config;
    private List<Species> _species = [];

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
        Console.WriteLine($"Gen:{Generation} ");
        //1. Get fitness of each genome
        foreach (var genome in Population)
        {
            var network = genome.BuildNeuralNetwork(Config.Activation);
            genome.Fitness = _evaluate(network);
            if (genome.Fitness == 0) Console.WriteLine($"Fitness:{genome.Fitness}");
        }
        
        
        //2. Elitism - pick _config.Elitism the best individuals that will automatically survive to the next generation
        //This way we never lose the best
        //Elitism that pick the best out of each species 
        List<Genome> elites = new();
        if (Config.SpeciateActive)
        {
            // Speciate population
            SpeciatePopulation();
            foreach (var species in _species)
            {
                var top = species.Members.OrderByDescending(g => g.Fitness).First();
                elites.Add(top.Clone());
            }
        }
        else
        { 
            //Normal elitism
            elites = Population
                .OrderByDescending(g => g.Fitness)
                .Take(_config.Elitism)
                .Select(g =>g.Clone())
                .ToList();  
        }
        
        //4. Select the rest of the population using Selector
        List<Genome> offspring = [];
        var remaining = Config.PopulationSize-elites.Count;
        if (Config.SpeciateActive)
        {
            {
                // Calculate adjusted fitness 
                var adjustedSpecies = _species.Select(s =>
                {
                    var totalFitness = s.Members.Sum(m => m.Fitness);
                    return (Species: s, Fitness: totalFitness);
                }).ToList();

                double fitnessSum = adjustedSpecies.Sum(t => t.Fitness);

                foreach (var (species, fitness) in adjustedSpecies)
                {
                    int offspringCount = (int)Math.Round((fitness / fitnessSum) * remaining);

                    for (int i = 0; i < offspringCount; i++)
                    {
                        var parent1 = species.Members[_random.Next(species.Members.Count)];
                        Genome child;

                        if (_random.NextDouble() < _config.CrossOverRate)
                        {
                            var parent2 = species.Members[_random.Next(species.Members.Count)];
                            child = Genome.Crossover(parent1, parent2, _random);
                        }
                        else
                        {
                            child = parent1.Clone();
                        }

                        child.Mutate(_config, _random);
                        offspring.Add(child);
                    }
                }
            }
        }
        else
        {
            //5. CrossOver and Mutation
            var selected = Selector(Population,remaining,_random);
            foreach (var child1 in selected)
            {

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

                //var newGenome = child1.Clone();
                newGenome.Mutate(_config,_random);
                offspring.Add(newGenome);

            }
        }
        Population = elites.Concat(offspring).ToList();
        Generation++;
    }
    
    public void RunGenerations(int count =Config.SynchronizationLength){
        for (int i = 0; i < count; i++)
        {
            RunGeneration();
        }
    }

    private void SpeciatePopulation()
    {
        _species.Clear();
        foreach (var genome in Population)
        {
            bool added = false;
            foreach (var species in _species)
            {
                if (genome.DistanceTo(species.Representative) < Config.SpeciationThreshold)
                {
                    species.AddMember(genome);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                _species.Add(new Species(genome));
            }
        }
        
    }
}