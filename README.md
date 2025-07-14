# 🚀 Rocket Learning 🚀
Rocket Learning is a reinforcement learning library using Neat and a 2D game simulation of landing a rocket simular to a lunar lander. 

## Features 
- **NEAT library** 
  - Evolving of a Neural Network using genetic programing (mutations and crossover).
  - NEAT library can be taken alone and used for other projects. All you need to supply will be a fittness function and set up config file.
- **Parallel training**
  - Training can run in parallel with periodic synchronization and mixing across the cores.
- **Game simulation**
  - Custom game simulation used for evaluating Agents. (Game does not have to run in real time only inputs from NN are being simulated).
- **Interactive Avalonia UI**
  - Play the game manually 
  - Train the NEAT algorithm
  - Showcase of the landings using NEAT

## NEAT configuration
In NEAT library in files config.cs you can configure the NEAT.
- int InputSize - number of inputs that the NN will be taking(Important to create the right amount of input nodes)
- int OutputSize - number of outputs the NN can choose from 


- int PopulationSize - how many genomes are created in each generation(they are then split in between all the cores)
- int NumberOfIterations - how many generations will be created 


- double AddNodeRate - Probability that a node will be added to a genome
- double RemoveNodeRate - Probability that a node will be added to a genome

- double StartingConnectionRate - Rate of how many out of all possible connections will be created when generating first generation
- double ReactivateRate - Probability of reactivating a connection that has been deactivated
- double AddConnectionRate -Probability that a connection will be added to a genome
- double RemoveConnectionRate -Probability that a connection will be deactivated in a genome 
- double WeightMutatePower - Rate by how much at most can the weight of connection change
- double WeightMutateRate - Probability that a weight in a genome will mutate
- double WeightChangeProbability - Probability that a weight will completely change and a new one will be created.
  

- int Elitism - How many out of the best genomes will be kept to survive and be moved into the next generation
- double CrossoverRate - How many genomes in the new generation will be created using crossover


- Activation - Activation function that can be chosen out of all activation functions in  ActivationFunction.cs
- Selection - Selection function - what selection method should be used in choosing genomes for the next generation can be found in SelectionFunction.cs


- bool Speciate - true if you want speciation to be active - crossover is done using speciation - genomes close together are put together
- int SynchronizationLength - After how many generations should all genomes be put together from all the parallel cores and redistributed again
- int Cores - how many cores should be used


## Agent 
It is possible to create your own agent and test it how good it performs in the game. 
- You will just need to create agent implementing interface IAgent with method RocketInput Decide(GameState state);
- In game state you will be supplied with all the information about the game and based on this you will need to return one of the RocketInputs(LeftMotor,None,RightMotor)
