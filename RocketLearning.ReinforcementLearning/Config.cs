namespace RocketLearning.ReinforcementLearning;

public class Config
{
    //Input output sizes 
    public const int InputSize = 7;
    public const int OutputSize = 3;
    
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
    
    //Does the NEAT try to maximise or minimise the fitness function 
    //Maybe this will not be used so be careful with this 
    public const bool Maximise = true;
    public bool Minimize = !Maximise;
    
    //Activation function - any function from ActivationFunction can be chosen ,or you can add a new one that you prefer
    public Func<double, double> Activation = ActivationFunction.Sigmoid;


}