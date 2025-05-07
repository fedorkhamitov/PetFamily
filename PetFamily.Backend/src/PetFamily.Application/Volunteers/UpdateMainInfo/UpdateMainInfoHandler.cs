using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    
    public UpdateMainInfoHandler(IVolunteersRepository volunteersRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken)
    {
        var volunteer = await _volunteersRepository
            .GetById(VolunteerId.Create(request.Id), cancellationToken);
        if (volunteer.IsFailure)
            return Error.Failure(volunteer.Error.Code, volunteer.Error.Message);
        var fullName = HumanName
            .Create(request.Dto.Name.FirstName, request.Dto.Name.SecondName, request.Dto.Name.LastName).Value;
        volunteer.Value.UpdateMainInfo(fullName, 
            request.Dto.Email, 
            request.Dto.Description,
            (ushort)request.Dto.YearsOfWorkExp,
            request.Dto.PhoneNumber);
        await _volunteersRepository.Save(volunteer.Value, cancellationToken);
        _logger.LogInformation("Updated main info for voluteer id: {0}, name: {1},{2}, phone: {3}",
            volunteer.Value.Id, volunteer.Value.Name.FirstName, volunteer.Value.Name.LastName, volunteer.Value.PhoneNumber);
        return volunteer.Value.Id.Value;
    }
}