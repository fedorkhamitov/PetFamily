using CSharpFunctionalExtensions;
using PetFamily.Domain.Infrastructure;
using PetFamily.Domain.Models;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    
    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, 
        CancellationToken cancellationToken = default)
    {
        SocialNetworkList? socialNetworkList = null;
        var socialNetworks = new List<SocialNetwork>(); 
        socialNetworks.AddRange(request.SocialNetworks.Select(l => 
            SocialNetwork.Create(l.Name, l.Url).Value));

        socialNetworkList = SocialNetworkList.Create(socialNetworks).Value;
        
        var volunteerId = VolunteerId.NewId();
        var name = HumanName.Create(
            request.VolunteerName.FirstName,
            request.VolunteerName.SecondName,
            request.VolunteerName.LastName
            ).Value;
        var volunteer = new Volunteer(
            volunteerId,
            name,
            request.Email,
            request.Description,
            request.YearsOfWorkExp,
            request.PhoneNumber,
            DonationDetails.Create(request.DonationDetails.Name, request.DonationDetails.Description).Value,
            socialNetworkList
            );
        await _volunteersRepository.Add(volunteer, cancellationToken);
        return volunteer.Id.Value;
    }
}