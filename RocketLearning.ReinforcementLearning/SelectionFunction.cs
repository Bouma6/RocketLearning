namespace RocketLearning.ReinforcementLearning;

public delegate List<Genome> SelectionDelegate(List<Genome> input,int populationSize,Random random);

public static class SelectionFunction
{
    //Roulette Selection - each genome has chance of being chosen equal to its fitness/total fitness
    //Works only when all the fitness scores are non-negative
    //Does not work well when the fitness of some is significantly larger than the fitness of the others 
    //If any fitness is negative returns RandomSelection
    public static List<Genome> RouletteSelection(List<Genome> input, int populationSize, Random random)
    {
        List<Genome> output = [];
        double totalWeight = 0;
        //Check whether all fitness scores are non-negative (negative fitness does not work for roulette selection) 
        for (var i = 0; i < populationSize; i++)
        {
            var fitness = input[i].Fitness;
            if (fitness < 0)
            {
                return RandomSelection(input, populationSize, random);    
            }
            totalWeight += fitness;
        }
        for (var i = 0; i < populationSize; i++)
        {
            double pick = random.NextDouble() * totalWeight;
            double cumulative = 0;
            foreach (var genome in input)
            {
                cumulative += genome.Fitness;
                if (!(cumulative <= pick)) continue;
                output.Add(genome);
                break;
            }
        }
        return output;
    }
    //Randomly chose tournamentSize individuals pick the best of them.
    //Usually the best option - fitness function can be negative and outliers are not a problem as well 
    public static List<Genome> TournamentSelection(List<Genome> input, int populationSize, Random random,int tournamentSize =3)
    {
        List<Genome> output = [];
        for (var i = 0; i < populationSize; i++)
        {
            //Pick tournamentSize amount of genomes
            var tournament = new List<Genome>();
            for (var j = 0; j < tournamentSize; j++)
            {
                tournament.Add(input[random.Next(input.Count)]);
            }
            //Select the fittest genome out from the tournament
            output.Add(tournament.OrderByDescending(g => g.Fitness).First());
        }
        return output;
    }
    //Randomly picks 
    public static List<Genome> RandomSelection(List<Genome> input, int populationSize, Random random)
    {
        List<Genome> output = [];
        for (var i = 0; i < populationSize; i++)
        {
            output.Add(input[random.Next(input.Count)]);
        }
        return output;
    }
}