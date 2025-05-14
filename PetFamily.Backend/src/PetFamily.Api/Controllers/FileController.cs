using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio;
using PetFamily.Api.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Modules;
using PetFamily.Domain.Share;
using PetFamily.Infrastructure.Options;

namespace PetFamily.Api.Controllers;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly IFileProvider _fileProvider;

    public FileController(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    [HttpPost("new")]
    public async Task<ActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();

        var fileMetaData = new MinioFileMetaData(Constants.BUCKET_NAME, Guid.NewGuid().ToString());

        var result = await _fileProvider.UploadFile(stream, fileMetaData, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpDelete("{fileName:guid}")]
    public async Task<ActionResult> RemoveFile([FromRoute] Guid fileName, CancellationToken cancellationToken)
    {
        var fileMetaData = new MinioFileMetaData(Constants.BUCKET_NAME, fileName.ToString());

        var result = await _fileProvider.DeleteFile(fileMetaData, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpGet("{fileName:guid}")]
    public async Task<ActionResult> GetByName([FromRoute] Guid fileName, CancellationToken cancellationToken)
    {
        var fileMetaData = new MinioFileMetaData(Constants.BUCKET_NAME, fileName.ToString());

        var result = await _fileProvider.GetFileByObjectName(fileMetaData, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }
}