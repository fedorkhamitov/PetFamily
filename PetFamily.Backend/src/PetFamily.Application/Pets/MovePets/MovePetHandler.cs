using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Pets.MovePets;

public class MovePetHandler(IVolunteersRepository volunteersRepository, ILogger<MovePetHandler> logger)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository;
    private readonly ILogger<MovePetHandler> _logger = logger;

    public async Task<Result<Guid, Error>> Handle(MovePetsCommand command, CancellationToken cancellationToken = default)
    {
        var volunteerResult =
            await _volunteersRepository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error;

        var oldPosition = petResult.Value.Position;
        
        var newPosition = Position.Create(command.NewPosition);
        if (newPosition.IsFailure)
            return newPosition.Error;

        var moveResult = volunteerResult.Value.MovePet(petResult.Value, newPosition.Value);
        if (moveResult.IsFailure)
            return moveResult.Error;
        
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Pet {0}, (id: {1}) was moved from {2} position to {3} position", 
            petResult.Value.Name, 
            petResult.Value.Id, 
            oldPosition.Value,
            petResult.Value.Position.Value);
        
        return volunteerResult.Value.Id.Value;
    }
}