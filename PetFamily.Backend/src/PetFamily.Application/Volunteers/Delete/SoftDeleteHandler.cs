using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.Delete;

public class SoftDeleteHandler(IVolunteersRepository volunteersRepository, 
    ILogger<SoftDeleteHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        DeleteRequest request,
        CancellationToken cancellationToken
    )
    {
        var volunteer = await volunteersRepository
            .GetById(VolunteerId.Create(request.Id), cancellationToken);
        if (volunteer.IsFailure)
            return Error.Failure(volunteer.Error.Code, volunteer.Error.Message);
        
        volunteer.Value.SoftDelete();
        
        var result = await volunteersRepository.Save(volunteer.Value, cancellationToken);
        logger.LogInformation("Volunteer id: {0} was soft deleted.", volunteer.Value.Id);
        return volunteer.Value.Id.Value;
    }
}