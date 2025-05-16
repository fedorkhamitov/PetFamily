namespace PetFamily.Application.Pets.UploadPetFiles;

public record UploadPetFileCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<FileDto> FilesDto);