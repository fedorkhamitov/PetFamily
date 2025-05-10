using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Volunteers.Delete;

public class HardDeleteHandler(IVolunteersRepository volunteersRepository, ILogger<HardDeleteHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        DeleteRequest request,
        CancellationToken cancellationToken)
    {
        var deletedVolunteer = await volunteersRepository
            .GetById(VolunteerId.Create(request.Id), cancellationToken);
        
        if (deletedVolunteer.IsFailure)
            return Error.Failure(deletedVolunteer.Error.Code, deletedVolunteer.Error.Message);
        
        var result = await volunteersRepository.HardDelete(deletedVolunteer.Value, cancellationToken);
        
        logger.LogInformation("Volunteer id: {0} was hard deleted.", deletedVolunteer.Value.Id);
        
        return deletedVolunteer.Value.Id.Value;
    }
}