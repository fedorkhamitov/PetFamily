namespace PetFamily.Application.Pets.MovePets;

public record MovePetsCommand(Guid VolunteerId, Guid PetId, int NewPosition);