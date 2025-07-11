namespace RocketLearning.ReinforcementLearning;

public class MasterTrainer
{
    private readonly Random _random = new Random();
    private readonly Config _config = new Config();

    private readonly List<Trainer> _trainers = [];

    public MasterTrainer(FitnessEvaluatorDelegate fitnessEvaluator)
    {
        List<Genome> fullPopulation = initializePopulation(Config.PopulationSize);
        
        int cores = _config.Cores;
        int perCore = fullPopulation.Count / cores;

        for (int i = 0; i < cores;i++)
        {
            var currentPopulation = fullPopulation
                .Skip(i * perCore)
                .Take(perCore)
                .Select(g =>g.Clone())
                .ToList();
            var trainer = new Trainer(currentPopulation, fitnessEvaluator, _config);
            _trainers.Add(trainer);
        }
    }

    public void Run(int generation = Config.NumberOfIterations)
    {
        
    }

    private void SynchronizePopulations()
    {
        
    }
    private List<Genome> initializePopulation(int genomeCount)
    {
        
    }
    
    
}