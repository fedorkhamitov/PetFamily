namespace PetFamily.Domain.Models;

public class Breed
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
}