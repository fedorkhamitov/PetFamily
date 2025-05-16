using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Modules;

public interface IFileProvider
{
    Task<UnitResult<Error>> UploadFile(IEnumerable<FileContent> fileContents, string bucketName,
        CancellationToken cancellationToken);
    Task<UnitResult<Error>> DeleteFile(List<MinioFileMetaData> filesMetaData, CancellationToken cancellationToken);

    Task<Result<string, Error>> GetFileByObjectName(MinioFileMetaData fileMetaData, 
            CancellationToken cancellationToken);
}