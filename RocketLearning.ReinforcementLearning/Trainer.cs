using System;

namespace RocketLearning.ReinforcementLearning;

public class Trainer
{
    public List<Genome> Population =[];
    private readonly Config _config;
    private readonly Func<NeuralNetwork, double> _evaluate;
    public int Generation { get; private set; } = 0;
    
    public Trainer(Config config, Func<NeuralNetwork, double> evaluate){}

}