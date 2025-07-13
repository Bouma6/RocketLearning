namespace RocketLearning.ReinforcementLearning;

public class Species
{
    public Genome Representative;
    public List<Genome> Members = [];

    public Species(Genome representative)
    {
        Representative = representative;
        Members.Add(representative);
    }

    public void AddMember(Genome member)
    {
        Members.Add(member);
    }

    public void Reset(Genome newRepresentative)
    {
        Representative = newRepresentative;
        Members.Clear();
        Members.Add(newRepresentative);
    }
}