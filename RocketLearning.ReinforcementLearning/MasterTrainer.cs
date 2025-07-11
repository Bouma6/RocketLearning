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
    
    
}