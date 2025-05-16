using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Response;
using PetFamily.Application.Modules;
using PetFamily.Application.Pets.DeletePetFiles;
using PetFamily.Application.Pets.UploadPetFiles;

namespace PetFamily.Api.Controllers;

[ApiController]
[Route("pet")]
public class PetController : ControllerBase
{
    private readonly IFileProvider _fileProvider;
    public PetController(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    [HttpPost("{volunteerId:guid}/photo/{petId:guid}")]
    public async Task<ActionResult> UploadPetFiles(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadPetFileHandler handler,
        CancellationToken cancellationToken)
    {
        List<FileDto> filesDto = [];

        try
        {
            filesDto.AddRange(from file in files 
                let stream = file.OpenReadStream() 
                select new FileDto(stream, file.FileName, file.ContentType));

            var command = new UploadPetFileCommand(volunteerId, petId, filesDto);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }
        finally
        {
            foreach (var fileDto in filesDto)
            {
                await fileDto.FileStream.DisposeAsync();
            }
        } 
    }

    [HttpDelete("{volunteerId:guid}/photo/{petId:guid}")]
    public async Task<ActionResult> DeletePetFiles(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetFilesRequest request,
        [FromServices] IValidator<DeletePetFilesRequest> validator,
        [FromServices] DeletePetFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var command = new DeletePetFilesCommand(request.FilePaths, volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(Envelope.Ok());
    }
}