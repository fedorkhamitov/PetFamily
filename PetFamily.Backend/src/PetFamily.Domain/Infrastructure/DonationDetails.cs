using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Infrastructure;

public record DonationDetails
{
    public string Name { get; } = default!;
    public string Description { get; } = default!;

    private DonationDetails()
    { }

    private DonationDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public Result<DonationDetails, Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsRequired("Name and Description");
        return new DonationDetails(name, description);
    }
}