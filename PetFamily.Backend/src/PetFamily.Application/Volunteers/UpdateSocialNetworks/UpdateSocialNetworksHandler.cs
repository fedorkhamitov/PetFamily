using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public class UpdateSocialNetworksHandler(IVolunteersRepository volunteersRepository,
    ILogger<UpdateSocialNetworksHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(UpdateSocialNetworksRequest request,
        CancellationToken cancellationToken)
    {
        var volunteer = await volunteersRepository
            .GetById(VolunteerId.Create(request.Id), cancellationToken);
        if (volunteer.IsFailure)
            return Error.Failure(volunteer.Error.Code, volunteer.Error.Message);
        var socialNetworks = (from dto in request.Dtos
            select SocialNetwork.Create(dto.Name, dto.Url).Value).ToList();
        volunteer.Value.UpdateSocialNetworks(socialNetworks);
        await volunteersRepository.Save(volunteer.Value, cancellationToken);
        logger.LogInformation("Updated Social Networks for voluteer id: {0}, name: {1},{2}, phone: {3}",
            volunteer.Value.Id, volunteer.Value.Name.FirstName, volunteer.Value.Name.LastName, volunteer.Value.PhoneNumber);
        return volunteer.Value.Id.Value;
    } 
}