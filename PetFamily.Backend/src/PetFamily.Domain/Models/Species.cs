using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public class Species : Entity
{
    public new Guid Id { get; private set; }
    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breeds { get; private set; }
    private Species(){}
    public Species(string name, IEnumerable<Breed> breeds)
    {
        Name = name;
        Breeds = breeds.ToList().AsReadOnly();
    }
}