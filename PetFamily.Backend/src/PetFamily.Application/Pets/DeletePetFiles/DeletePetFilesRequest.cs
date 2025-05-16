using PetFamily.Domain.Share;

namespace PetFamily.Application.Pets.DeletePetFiles;

public record DeletePetFilesRequest(IEnumerable<FilePath> FilePaths);