using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    HumanNameRequestDto VolunteerName,
    string Email,
    string Description,
    ushort YearsOfWorkExp,
    string PhoneNumber,
    DonationDetailsRequestDto DonationDetails,
    IEnumerable<SocialNetworksDto> SocialNetworks);