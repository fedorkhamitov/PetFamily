namespace PetFamily.Application.Pets.UploadPetFiles;

public record FileDto(Stream FileStream, string FileName, string ContentType);