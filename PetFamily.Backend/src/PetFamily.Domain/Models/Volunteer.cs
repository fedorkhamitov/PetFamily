using CSharpFunctionalExtensions;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Domain.Models;

public class Volunteer : Entity
{
    public VolunteerId Id { get; private set; }
    public HumanName Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public ushort YearsOfWorkExp { get; private set; }
    public IReadOnlyList<Pet> Pets { get; private set; } = default!;
    public int FoundHomePetsCount => Pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);
    public int LookingHomePetsCount => Pets.Count(p => p.HelpStatus == HelpStatus.LookingHome);
    public int NeedHelpPetsCount => Pets.Count(p => p.HelpStatus == HelpStatus.NeedHelp);
    public string PhoneNumber { get; private set; } = default!;
    public SocialNetworkList? SocialNetworkList { get; private set; }
    public DonationDetails? Donation { get; private set; }
    
    private Volunteer(){}

    public Volunteer(
        VolunteerId id,
        HumanName name,
        string email,
        string description,
        ushort yearsOfWorkExp,
        string phoneNumber,
        DonationDetails? donation,
        SocialNetworkList? socialNetworks
        )
    {
        Id = id;
        Name = name;
        Email = email;
        Description = description;
        YearsOfWorkExp = yearsOfWorkExp;
        PhoneNumber = phoneNumber;
        Donation = donation;
        SocialNetworkList = socialNetworks;
    }

    public void UpdateMainInfo(
        HumanName name,
        string email,
        string description,
        ushort yearsOfWorkExp,
        string phoneNumber)
    {
        Name = name;
        Email = email;
        Description = description;
        YearsOfWorkExp = yearsOfWorkExp;
        PhoneNumber = phoneNumber;
    }

    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        var socialNetworkList = SocialNetworkList.Create(socialNetworks.ToList()).Value;
        SocialNetworkList = socialNetworkList;
    }

    public void UpdateDonationDetails(DonationDetails donationDetails)
    {
        Donation = donationDetails;
    }
}