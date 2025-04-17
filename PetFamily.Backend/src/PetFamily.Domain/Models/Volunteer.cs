using CSharpFunctionalExtensions;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Domain.Models;

public class Volunteer : Entity
{
    public new Guid Id { get; private set; }
    public HumanName Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public ushort YearsOfWorkExp { get; private set; }
    public IReadOnlyList<Pet> Pets { get; private set; } = default!;
    public int FoundHomePetsCount => Pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);
    public int LookingHomePetsCount => Pets.Count(p => p.HelpStatus == HelpStatus.LookingHome);
    public int NeedHelpPetsCount => Pets.Count(p => p.HelpStatus == HelpStatus.NeedHelp);
    public string PhoneNumber { get; private set; } = default!;
    public IReadOnlyList<SocialNetwork> SocialNetworkList { get; private set; } = default!;
    public DonationDetails Donation { get; private set; }
    
    private Volunteer(){}

    public Volunteer(
        HumanName name,
        string email,
        string description,
        ushort yearsOfWorkExp,
        string phoneNumber,
        DonationDetails donation,
        IEnumerable<Pet> pets,
        IEnumerable<SocialNetwork> socialNetworks
        )
    {
        Id = new Guid();
        Name = name;
        Email = email;
        Description = description;
        YearsOfWorkExp = yearsOfWorkExp;
        PhoneNumber = phoneNumber;
        Donation = donation;
        Pets = pets.ToList().AsReadOnly();
        SocialNetworkList = socialNetworks.ToList().AsReadOnly();
    }
}