namespace PetFamily.Domain.Infrastructure;

public record SocialNetworkList
{
    public IReadOnlyList<SocialNetwork> SnList { get; private set; } = default!;
}