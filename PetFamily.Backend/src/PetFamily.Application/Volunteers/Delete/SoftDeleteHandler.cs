using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Volunteers.Delete;

public class SoftDeleteHandler(IVolunteersRepository volunteersRepository, 
    ILogger<SoftDeleteHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        DeleteRequest request,
        CancellationToken cancellationToken)
    {
        var deletedVolunteer = await volunteersRepository
            .GetById(VolunteerId.Create(request.Id), cancellationToken);
        
        if (deletedVolunteer.IsFailure)
            return Error.Failure(deletedVolunteer.Error.Code, deletedVolunteer.Error.Message);
        
        deletedVolunteer.Value.Delete();
        
        var result = await volunteersRepository.Save(deletedVolunteer.Value, cancellationToken);
        
        logger.LogInformation("Volunteer id: {0} was soft deleted.", deletedVolunteer.Value.Id);
        
        return deletedVolunteer.Value.Id.Value;
    }
}