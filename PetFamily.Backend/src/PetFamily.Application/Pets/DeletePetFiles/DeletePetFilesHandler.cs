using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Modules;
using PetFamily.Application.Pets.UploadPetFiles;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Pets.DeletePetFiles;

public class DeletePetFilesHandler(
    IFileProvider fileProvider,
    IVolunteersRepository volunteersRepository,
    ILogger<UploadPetFileHandler> logger)
{
    public async Task<UnitResult<Error>> Handle(
        DeletePetFilesCommand command,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await volunteersRepository.GetById(VolunteerId.Create(command.VolunteerId),
            cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error;

        var minioFilesMetaData = command.Paths.Select(filepath => 
                new MinioFileMetaData(Constants.PHOTOS_BUCKET_NAME, filepath.Path)).ToList();

        var deleteResult = await fileProvider.DeleteFile(minioFilesMetaData, cancellationToken);
        if (deleteResult.IsFailure)
            return deleteResult.Error;
        
        var deletedPetFiles = command.Paths.Select(filepath =>
            PetFile.Create(filepath).Value);
        
        var result = petResult.Value.RemoveFiles(deletedPetFiles);
        if (result.IsFailure)
            return result.Error;
        
        await volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        foreach (var petFile in deletedPetFiles)
        {
            logger.LogInformation("Created new file : {0}", petFile.PathToStorage);
        }
        
        return Result.Success<Error>();
    }
}