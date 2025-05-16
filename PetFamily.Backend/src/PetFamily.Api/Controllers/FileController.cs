using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio;
using PetFamily.Api.Extensions;
using PetFamily.Api.Response;
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

        var fileName = Guid.NewGuid().ToString();

        var fileContent = new FileContent(stream, fileName);
        
        var result = await _fileProvider.UploadFile([ fileContent ], Constants.PHOTOS_BUCKET_NAME, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok();
    }

    [HttpDelete("{fileName:guid}")]
    public async Task<ActionResult> RemoveFile([FromRoute] Guid fileName, CancellationToken cancellationToken)
    {
        var fileMetaData = new MinioFileMetaData(Constants.PHOTOS_BUCKET_NAME, fileName.ToString());

        var result = await _fileProvider.DeleteFile([fileMetaData], cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(Envelope.Ok());
    }

    [HttpGet("{fileName:guid}")]
    public async Task<ActionResult> GetByName([FromRoute] Guid fileName, CancellationToken cancellationToken)
    {
        var fileMetaData = new MinioFileMetaData(Constants.PHOTOS_BUCKET_NAME, fileName.ToString());

        var result = await _fileProvider.GetFileByObjectName(fileMetaData, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }
}