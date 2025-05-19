using PetFamily.Domain.Share;

namespace PetFamily.Application.Pets.DeletePetFiles;

public record DeletePetFilesCommand(IEnumerable<FilePath> Paths, Guid VolunteerId, Guid PetId);