namespace RocketLearning.ReinforcementLearning;

public class Config
{
    //Input output sizes 
    public const int InputSize = 7;
    public const int OutputSize = 3;
    
    //Population
    public const int PopulationSize = 100;
    public const int NumberOfIterations = 1000;
    
    
    //ID of output layer nodes and bias node 
    public const int BiasNodeId = 999;
    public const int OutputLeftId = 1000;
    public const int OutputRightId = 1001;
    public const int OutputNoneId = 1002;
    
    //May change during the run of NEAT as sometimes it may be beneficial when MutationRate decreases over time 
    //Probability that a node will be added/removed 
    public double AddNodeRate = 0.1;
    public double RemoveNodeRate = 0.1;    
    
    //Connections
    //Probability that a connectino will be added/removed 
    public double AddConnection = 0.2;
    public double RemoveConnection = 0.2;
    //Max/min weight of connections
    public const double MaxValue = 30;
    public const double MinValue = -30;
    //When a weight mutates what is the maximum factor the weight can mutate by +-(random number from gaussian distribution * connection Mutate Power )
    public const double ConnectionMutatePower = 0.5;
    //Completely new number will be generated 
    public const double ConnectionChangeProbability = 0.1;
    
    //enable/disable connection probability 
    public const double EnableConnectionProbability = 0.01;
    
    //How many of the best individuals from previous generation you want to survive into the next generation.
    public int Elitism = 3; 
    
    //Activation function - any function from ActivationFunction can be chosen ,or you can add a new one that you prefer
    public static ActivationDelegate Activation = ActivationFunction.Sigmoid;
    public static SelectionDelegate Selection = SelectionFunction.RouletteSelection;

    //PARA
    public bool Parallel = true;
    //After how many generations all the threads should wait for each other to redistribute the individuals
    public int SynchronizationLength = 10;
    public int Cores = Environment.ProcessorCount;
    
    
    
}