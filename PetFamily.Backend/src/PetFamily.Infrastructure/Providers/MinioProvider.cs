using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Modules;
using PetFamily.Domain.Share;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger) : IFileProvider
{
    public async Task<UnitResult<Error>> UploadFile(IEnumerable<FileContent> fileContents, string bucketName,
        CancellationToken cancellationToken)
    {
        var semaphoreSlim = new SemaphoreSlim(Constants.MAX_PARALLEL_TASK_LENGTH);
        try
        {
            var isBucketExist = await IsBucketExist(bucketName, cancellationToken);
            if (isBucketExist.IsFailure)
                return isBucketExist.Error;
            if (isBucketExist.Value == false)
            {
                var creatingBucket = await CreateBucket(bucketName, cancellationToken);
                if (creatingBucket.IsFailure)
                    return creatingBucket.Error;
            }

            List<Task> tasks = [];
            foreach (var fileContent in fileContents)
            {
                await semaphoreSlim.WaitAsync(cancellationToken);

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithStreamData(fileContent.FileStream)
                    .WithObjectSize(fileContent.FileStream.Length)
                    .WithObject(fileContent.FileName);

                var task = minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                semaphoreSlim.Release();

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to upload file minio");
            return Error.Failure("file.upload", "Fail to upload file minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    public async Task<UnitResult<Error>> DeleteFile(List<MinioFileMetaData> filesMetaData,
        CancellationToken cancellationToken)
    {
        var semaphoreSlim = new SemaphoreSlim(Constants.MAX_PARALLEL_TASK_LENGTH);
        List<Task> tasks = [];
        try
        {
            foreach (var fileMetaData in filesMetaData)
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(fileMetaData.BucketName)
                    .WithObject(fileMetaData.ObjectName);
                var task = minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
                semaphoreSlim.Release();
                tasks.Add(task);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Fail to remove file minio");
            return Error.Failure("file.remove", "Fail to remove file minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
        
        await Task.WhenAll(tasks);
        
        return Result.Success<Error>();
    }

    public async Task<Result<string, Error>> GetFileByObjectName(MinioFileMetaData fileMetaData,
        CancellationToken cancellationToken)
    {
        var isBucketExist = await IsBucketExist(fileMetaData.BucketName, cancellationToken);
        if (isBucketExist.IsFailure)
            return isBucketExist.Error;
        if (isBucketExist.Value == false)
            return Error.NotFound("bucket.exist", "Bucket is not exist");

        var presignedObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(fileMetaData.BucketName)
            .WithObject(fileMetaData.ObjectName)
            .WithExpiry(Constants.MINIO_EXPIRED_TIME);

        var result = await minioClient.PresignedGetObjectAsync(presignedObjectArgs);
        if (result == null)
            return Error.Failure("file.get", "Fail to presigned get object");
        return result;
    }

    private async Task<Result<bool, Error>> IsBucketExist(string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            var result = await minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail bucket exist minio");
            return Error.Failure("bucket.exist", "Fail bucket exist minio");
        }
    }

    private async Task<UnitResult<Error>> CreateBucket(string bucketName, CancellationToken cancellationToken)
    {
        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(bucketName);
        try
        {
            await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            logger.LogInformation("Created new minio bucket: {0}", bucketName);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail create bucket minio");
            return Error.Failure("bucket.create", "Fail create bucket minio");
        }
    }
}