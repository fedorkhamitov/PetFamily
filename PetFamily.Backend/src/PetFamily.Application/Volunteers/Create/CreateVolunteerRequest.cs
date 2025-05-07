namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    HumanNameRequestDto VolunteerName,
    string Email,
    string Description,
    ushort YearsOfWorkExp,
    string PhoneNumber,
    DonationDetailsRequestDto DonationDetails,
    IEnumerable<SocialNetworksDto> SocialNetworks);