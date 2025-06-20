using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Response;
using PetFamily.Application.Pets.DeletePetFiles;
using PetFamily.Application.Pets.MovePets;
using PetFamily.Application.Pets.UploadPetFiles;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateDonationDetails;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.Api.Controllers;

[ApiController]
[Route("volunteer")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromServices] IValidator<CreateVolunteerRequest> validator,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        var result = await handler.Handle(request, cancellationToken);
        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo
    (
        [FromRoute] Guid id,
        [FromServices] UpdateMainInfoHandler handler,
        [FromBody] UpdateMainInfoDto dto,
        [FromServices] IValidator<UpdateMainInfoDto> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateMainInfoRequest(id, dto);
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        var result = await handler.Handle(request, cancellationToken);
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromServices] UpdateSocialNetworksHandler handler,
        [FromServices] IValidator<UpdateSocialNetworksRequest> validator,
        [FromBody] IEnumerable<UpdateSocialNetworksDto> dtos,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateSocialNetworksRequest(id, dtos);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        var result = await handler.Handle(request, cancellationToken);
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/donation-details")]
    public async Task<ActionResult<Guid>> UpdateDonationDetails(
        [FromRoute] Guid id,
        [FromServices] UpdateDonationDetailsHandler handler,
        [FromServices] IValidator<UpdateDonationDetailsRequest> validator,
        [FromBody] UpdateDonationDetailsDto dto,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateDonationDetailsRequest(id, dto);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        var result = await handler.Handle(request, cancellationToken);
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> HardDelete(
        [FromRoute] Guid id,
        [FromServices] HardDeleteHandler handler,
        [FromServices] IValidator<DeleteRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteRequest(id);
        
        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpDelete("{id:guid}/soft")]
    public async Task<ActionResult<Guid>> SoftDelete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteRequest(id);
        
        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPut("{id:guid}/active")]
    public async Task<ActionResult<Guid>> Restore(
        [FromRoute] Guid id,
        [FromServices] RestoreHandler handler,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteRequest(id);
        
        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }

    [HttpPost("{id:guid}/new-pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        [FromServices] IValidator<AddPetRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        
        var command = new AddPetCommand(
            id,
            request.Name,
            request.Description,
            request.IsSterilized,
            request.IsVaccinated,
            request.SpeciesId,
            request.BreedId,
            request.Color,
            request.Address,
            request.Weight,
            request.Height,
            request.PhoneNumber,
            request.BirthDate,
            request.DonationDetails,
            request.Position);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }
    
    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> UploadPetFiles(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadPetFileHandler handler,
        CancellationToken cancellationToken = default)
    {
        List<FileDto> filesDto = [];

        try
        {
            filesDto.AddRange(from file in files 
                let stream = file.OpenReadStream() 
                select new FileDto(stream, file.FileName, file.ContentType));

            var command = new UploadPetFileCommand(volunteerId, petId, filesDto);

            var result = await handler.Handle(command, cancellationToken);
            return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok());
        }
        finally
        {
            foreach (var fileDto in filesDto)
            {
                await fileDto.FileStream.DisposeAsync();
            }
        } 
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> DeletePetFiles(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetFilesRequest request,
        [FromServices] IValidator<DeletePetFilesRequest> validator,
        [FromServices] DeletePetFilesHandler handler,
        CancellationToken cancellationToken = default)
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

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/new-position-{position:int}")]
    public async Task<ActionResult> MovePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromRoute] int position,
        [FromServices] MovePetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new MovePetsCommand(volunteerId, petId, position);

        var result = await handler.Handle(command, cancellationToken);
        
        return result.IsFailure ? BadRequest(result.Error) : Ok(Envelope.Ok());
    }
}