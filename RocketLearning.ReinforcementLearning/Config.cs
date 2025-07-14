namespace RocketLearning.ReinforcementLearning;

public class Config
{
    //Input output sizes 
    public const int InputSize = 5;
    public const int OutputSize = 3;
    
    //Population
    public const int PopulationSize = 600;
    public const int NumberOfIterations = 400;
    
    
    //ID of output layer nodes and bias node 
    public const int OutputNodesId = 1000;
    public const int BiasNodeId = 999;
    
    //May change during the run of NEAT as sometimes it may be beneficial when MutationRate decreases over time 
    //Probability that a node will be added/removed 
    public double AddNodeRate = 0.2;
    public double RemoveNodeRate = 0.2;
    public const double StartingConnectionsRate = 0.4;
    public double ReactivateRate = 0.3;
    
    //Connections
    //Probability that a connectino will be added/removed 
    public double AddConnectionRate = 0.3;
    public double RemoveConnectionRate = 0.2;
    
    
    //Distance Function
    // c1 = excess gene coefficient
    // c2 = disjoint gene coefficient
    // c3 = average weight difference coefficient
    public const double ExcessGeneCoefficient = 1;
    public const double DisjointGeneCoefficient = 1;
    public const double AverageWeightDifferenceCoefficient = 0.4;
    public const double SpeciationThreshold = 3.0;
    
    //Max/min weight of connections
    public const double MaxWeight = 1000;
    public const double MinWeight = -1000;
    //When a weight mutates what is the maximum factor the weight can mutate by +-(random number from gaussian distribution * connection Mutate Power )
    public double WeightMutatePower = 0.4;
    //Weight mutation rate
    public double WeightMutateRate = 0.4;
    //Completely new number will be generated 
    public double WeightChangeProbability = 0.3;
    
    //enable/disable connection probability 
    public const double EnableConnectionRate = 0.1;
        
    //How many of the best individuals from previous generation you want to survive into the next generation.
    public int Elitism = 3; 
    //The Rate of how many of the offsprings will be created by crossover and how many will be kept untouched 
    public double CrossOverRate = 0.2;
    
    //Activation function - any function from ActivationFunction can be chosen ,or you can add a new one that you prefer
    public static ActivationDelegate Activation = ActivationFunction.Sigmoid;
    public static SelectionDelegate Selection = SelectionFunction.TournamentSelection;

    public const bool SpeciateActive = false;
    //PARA
    //After how many generations all the threads should wait for each other to redistribute the individuals
    //Do not be a dick and make it a divisor of Number of Generations 
    public const int SynchronizationLength = 25;
    public int Cores = 8;
}