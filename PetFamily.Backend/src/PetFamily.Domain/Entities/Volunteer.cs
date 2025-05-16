using CSharpFunctionalExtensions;
using PetFamily.Domain.Share;

namespace PetFamily.Domain.Entities;

public class Volunteer : SoftDeletableEntity
{
    public VolunteerId Id { get; private set; }
    public HumanName Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public ushort YearsOfWorkExp { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
    private readonly List<Pet> _pets = [];
    public int FoundHomePetsCount => Pets!.Count(p => p.HelpStatus == HelpStatus.FoundHome);
    public int LookingHomePetsCount => Pets!.Count(p => p.HelpStatus == HelpStatus.LookingHome);
    public int NeedHelpPetsCount => Pets!.Count(p => p.HelpStatus == HelpStatus.NeedHelp);
    public string PhoneNumber { get; private set; } = default!;
    public SocialNetworkList? SocialNetworkList { get; private set; }
    public DonationDetails? Donation { get; private set; }

    private Volunteer()
    {
    }

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

    public override void Delete()
    {
        base.Delete();
        if (Pets.Count == 0) return;
        foreach (var pet in Pets)
        {
            pet.Delete();
        }
    }

    public override void Restore()
    {
        base.Restore();
        if (Pets.Count == 0) return;
        foreach (var pet in Pets)
        {
            pet.Restore();
        }
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var position = Position.Create(Pets.Count + 1);
        if (position.IsFailure)
            return position.Error;

        pet.SetPosition(position.Value);

        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public Result<Pet, Error> GetPetById(Guid petId)
    {
        var result = Pets.FirstOrDefault(p => p.Id == petId);
        if (result is null) return Errors.General.NotFound(petId);
        return result;
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;

        if (newPosition == currentPosition || Pets.Count == 1)
            return Result.Success<Error>();

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;

        newPosition = adjustedPosition.Value;

        var moveResult = MovePetsBetweenPositions(currentPosition, newPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;

        pet.SetPosition(newPosition);

        return Result.Success<Error>();
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position currentPosition, Position newPosition)
    {
        if (newPosition.Value < currentPosition.Value)
        {
            var petsToMove = Pets.Where(p => p.Position.Value >= newPosition.Value
                                             && p.Position.Value < currentPosition.Value);

            foreach (var movingPet in petsToMove)
            {
                var result = movingPet.MoveForward();
                if (result.IsFailure)
                    return result.Error;
            }
        }
        else if (newPosition.Value > currentPosition.Value)
        {
            var petsToMove = Pets.Where(p => p.Position.Value <= newPosition.Value
                                             && p.Position.Value > currentPosition.Value);

            foreach (var movingPet in petsToMove)
            {
                var currentPositionValue = movingPet.Position.Value;
                var result = movingPet.MoveBack();
                if (result.IsFailure)
                    return result.Error;
            }
        }

        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value < Pets.Count)
            return newPosition;

        if (newPosition.Value < Position.First.Value)
            return Position.First;

        var lastPosition = Position.Create(Pets.Count());
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }
}