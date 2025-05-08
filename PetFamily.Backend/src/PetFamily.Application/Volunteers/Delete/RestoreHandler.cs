using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Volunteers.Delete;

public class RestoreHandler(IVolunteersRepository volunteersRepository, 
    ILogger<SoftDeleteHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(DeleteRequest request, CancellationToken cancellationToken)
    {
        var restoreVolunteer = await volunteersRepository
            .GetById(VolunteerId.Create(request.Id), cancellationToken);
        
        if (restoreVolunteer.IsFailure)
            return Error.Failure(restoreVolunteer.Error.Code, restoreVolunteer.Error.Message);
        
        restoreVolunteer.Value.Restore();
        
        var result = await volunteersRepository.Save(restoreVolunteer.Value, cancellationToken);
        
        logger.LogInformation("Volunteer id: {0} was restore.", restoreVolunteer.Value.Id);
        
        return restoreVolunteer.Value.Id.Value;
    }
}