using CSharpFunctionalExtensions;
using PetFamily.Domain.Share;

namespace PetFamily.Domain.Entities;

public class Pet : SoftDeletableEntity
{
    public new Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public Position Position { get; private set; }
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
    public HelpStatus HelpStatus { get; private set; }
    public IReadOnlyList<PetFile> Files => _files;
    private readonly List<PetFile> _files = [];
    
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
        Position position,
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
        HelpStatus = helpStatus;
        Position = position;
    }
 
    public void SetPosition(Position position) 
        => Position = position;

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> AddFile(PetFile petFile)
    {
        _files.Add(petFile);
        return Result.Success<Error>();
    }

    public UnitResult<Error> AddFiles(IEnumerable<PetFile> petFiles)
    {
        _files.AddRange(petFiles);
        return Result.Success<Error>();
    }

    public UnitResult<Error> RemoveFiles(IEnumerable<PetFile> petFiles)
    {
        foreach (var petFile in petFiles)
        {
            _files.Remove(petFile);
        }
        
        return Result.Success<Error>();
    }
}