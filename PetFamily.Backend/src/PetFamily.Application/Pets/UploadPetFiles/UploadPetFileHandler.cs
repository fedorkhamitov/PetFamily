using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Modules;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Pets.UploadPetFiles;

public class UploadPetFileHandler(
    IFileProvider fileProvider,
    IVolunteersRepository volunteersRepository,
    ILogger<UploadPetFileHandler> logger)
{
    public async Task<UnitResult<Error>> Handle(
        UploadPetFileCommand command,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await volunteersRepository.GetById(VolunteerId.Create(command.VolunteerId),
            cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error;

        List<FileContent> filesContent = [];
        List<PetFile> petFiles = [];
        foreach (var fileDto in command.FilesDto)
        {
            var filePath = FilePath.Create(fileDto.FileName);
            if (filePath.IsFailure)
                return filePath.Error;
            petFiles.Add(PetFile.Create(filePath.Value).Value);
            
            filesContent.Add(new FileContent(fileDto.FileStream, fileDto.FileName));
        }

        var uploadResult = await fileProvider.UploadFile(
            filesContent, 
            Constants.PHOTOS_BUCKET_NAME, 
            cancellationToken);

        if (uploadResult.IsFailure)
            return uploadResult.Error;

        
        petResult.Value.AddFiles(petFiles);
        
        await volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        foreach (var petFile in petFiles)
        {
            logger.LogInformation("Created new file : {0}", petFile.PathToStorage);
        }
        
        return Result.Success<Error>();
    }
}