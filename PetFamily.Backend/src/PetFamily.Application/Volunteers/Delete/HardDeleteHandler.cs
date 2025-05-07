using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.Delete;

public class HardDeleteHandler(IVolunteersRepository volunteersRepository, ILogger<HardDeleteHandler> logger)
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
        var result = await volunteersRepository.HardDelete(volunteer.Value, cancellationToken);
        logger.LogInformation("Volunteer id: {0} was hard deleted.", volunteer.Value.Id);
        return volunteer.Value.Id.Value;
    }
}