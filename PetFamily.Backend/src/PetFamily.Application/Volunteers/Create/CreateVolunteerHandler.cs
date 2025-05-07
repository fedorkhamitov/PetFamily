using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Infrastructure;
using PetFamily.Domain.Models;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    
    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
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
        _logger.LogInformation("Created new voluteer id: {0}, name: {1},{2}, phone: {3}",
            volunteer.Id.Value, volunteer.Name.FirstName, volunteer.Name.LastName, volunteer.PhoneNumber);
        return volunteer.Id.Value;
    }
}