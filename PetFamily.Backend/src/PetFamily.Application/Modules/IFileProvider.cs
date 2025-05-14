using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Modules;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(Stream fileStream, MinioFileMetaData fileData, 
        CancellationToken cancellationToken);
    Task<Result<string, Error>> DeleteFile(MinioFileMetaData fileMetaData, CancellationToken cancellationToken);

    Task<Result<string, Error>> GetFileByObjectName(MinioFileMetaData fileMetaData, 
            CancellationToken cancellationToken);
}