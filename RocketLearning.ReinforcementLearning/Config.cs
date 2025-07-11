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
    public const int OutputNodesId = 1000;
    public const int BiasNodeId = 999;
    
    //May change during the run of NEAT as sometimes it may be beneficial when MutationRate decreases over time 
    //Probability that a node will be added/removed 
    public double AddNodeRate = 0.1;
    public double RemoveNodeRate = 0.1;
    public const double StartingConectionsRate = 0.3;
    public double ReactivateRate = 0.17;
    
    //Connections
    //Probability that a connectino will be added/removed 
    public double AddConnectionRate = 0.05;
    public double RemoveConnectionRate = 0.2;
    
    
    
    //Max/min weight of connections
    public const double MaxWeight = 30;
    public const double MinWeight = -30;
    //When a weight mutates what is the maximum factor the weight can mutate by +-(random number from gaussian distribution * connection Mutate Power )
    public double WeightMutatePower = 0.5;
    //Weight mutation rate
    public double WeightMutateRate = 0.5;
    //Completely new number will be generated 
    public double WeightChangeProbability = 0.1;
    //Minimum connections
    public const int MinimumConnections = 2;
    
    //enable/disable connection probability 
    public const double EnableConnectionRate = 0.01;
    
    //How many of the best individuals from previous generation you want to survive into the next generation.
    public int Elitism = 3; 
    
    //Activation function - any function from ActivationFunction can be chosen ,or you can add a new one that you prefer
    public static ActivationDelegate Activation = ActivationFunction.Sigmoid;
    public static SelectionDelegate Selection = SelectionFunction.RouletteSelection;
    
    //PARA
    public bool Parallel = true;
    //After how many generations all the threads should wait for each other to redistribute the individuals
    //Do not be a dick and make it a divisor of Number of Generations 
    public const int SynchronizationLength = 10;
    public int Cores = Environment.ProcessorCount;
}