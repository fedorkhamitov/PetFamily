using CSharpFunctionalExtensions;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Domain.Models;

public class Pet : Entity
{
    private bool _isDeleted = false;
    public new Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public bool IsSterilized { get; private set; }
    public bool IsVaccinated { get; private set; }
    public SpeciesInfo SpeciesInfo { get; private set; }
    public string Color { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public int Weight { get; private set; }
    public int Height { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public DateTime BirthDate { get; private set; }
    public DonationDetails DonationDetails { get; private set; } = default!;
    public DateTime CreatedDate { get; private set; }
    public HelpStatus HelpStatus { get; private set; } = default;
    
    private Pet(){}

    public Pet(
        string name,
        string description,
        bool isSterilized,
        bool isVaccinated,
        SpeciesInfo species,
        string color,
        Address address,
        int weight,
        int height,
        string phoneNumber,
        DateTime birthDate,
        DonationDetails donationDetails,
        HelpStatus helpStatus = HelpStatus.NeedHelp
    )
    {
        Id = new Guid();
        CreatedDate = DateTime.UtcNow;
        Name = name;
        Description = description;
        IsSterilized = isSterilized;
        IsVaccinated = isVaccinated;
        SpeciesInfo = species;
        Color = color;
        Address = address;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
        DonationDetails = donationDetails;
    }
    public void SoftDelete() => _isDeleted = true;
    public void Restore() => _isDeleted = false;
}