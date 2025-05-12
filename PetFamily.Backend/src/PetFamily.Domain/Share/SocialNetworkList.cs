using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Share;

public record SocialNetworkList
{
    public IReadOnlyList<SocialNetwork> SnList { get; } = default!;

    private SocialNetworkList() { }

    private SocialNetworkList(List<SocialNetwork> list)
    {
        SnList = list;
    }

    public static Result<SocialNetworkList, Error> Create(List<SocialNetwork>? list = null)
    {
        if (list is null) return Errors.General.ValueIsInvalid("Social Network List");
        return new SocialNetworkList(list);
    }
}