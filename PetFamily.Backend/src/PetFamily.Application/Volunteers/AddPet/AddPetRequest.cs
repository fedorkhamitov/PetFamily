using PetFamily.Domain.Share;

namespace PetFamily.Application.Volunteers.AddPet;

public record AddPetRequest(
    string Name,
    string Description,
    bool IsSterilized,
    bool IsVaccinated,
    Guid SpeciesId,
    Guid BreedId,
    string Color,
    AddressDto Address,
    int Weight,
    int Height,
    string PhoneNumber,
    DateTime BirthDate,
    DonationDetailsDto DonationDetails,
    int Position,
    HelpStatus HelpStatus = HelpStatus.NeedHelp);