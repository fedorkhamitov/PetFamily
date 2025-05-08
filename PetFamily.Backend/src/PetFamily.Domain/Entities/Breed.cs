using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities;

public class Breed : Entity
{
    public new Guid Id { get; private set; }
    public string Name { get; private set; }

    private Breed()
    {
    }

    public Breed(string name)
    {
        Name = name;
    }
}