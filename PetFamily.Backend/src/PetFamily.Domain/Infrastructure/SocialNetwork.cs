using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Infrastructure;

public record SocialNetwork
{
    public string Name { get; } = default!;
    public string Url { get; } = default!;

    private SocialNetwork()
    { }

    private SocialNetwork(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public static Result<SocialNetwork, Error> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(url))
            return Errors.General.ValueIsRequired("Name and URL");
        return new SocialNetwork(name, url);
    }
}