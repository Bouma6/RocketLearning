namespace RocketLearning.ReinforcementLearning;

public class MasterTrainer
{
    private readonly Random _random = new Random();
    private readonly Config _config = new Config();

    private readonly List<Trainer> _trainers = [];

    public MasterTrainer(FitnessEvaluatorDelegate fitnessEvaluator)
    {
        InitializePopulation(Config.PopulationSize);
        
        _cores = _config.Cores;
        _perCore = Config.PopulationSize / _cores;
        _evaluator = fitnessEvaluator;

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
        int syncEvery = Config.SynchronizationLength;
        int syncSteps = generation / syncEvery;

        for (int i = 0; i < syncSteps; i++)
        {
            Parallel.ForEach(_trainers, trainer =>
            {
                trainer.RunGenerations(syncEvery);
            });
            SynchronizePopulations();
        }
    }

    private void SynchronizePopulations()
    {
        
    }
    private List<Genome> initializePopulation(int genomeCount)
    {
        
    }
    
    
}