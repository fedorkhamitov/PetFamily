using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Response;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.UpdateDonationDetails;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;
using PetFamily.Domain.Infrastructure;
using Serilog;

namespace PetFamily.Api.Controllers;

[ApiController]
[Route("[controller]")]
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
        Log.Information("Created volunteer {0}", result.Value);
        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpPut("/{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo
    (
        [FromRoute] Guid id,
        [FromServices] UpdateMainInfoHandler handler,
        [FromBody] UpdateMainInfoDto dto,
        [FromServices] IValidator<UpdateMainInfoDto> validator,
        CancellationToken cancellationToken
    )
    {
        var request = new UpdateMainInfoRequest(id, dto);
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        var result = await handler.Handle(request, cancellationToken);
        Log.Information("Volunteer id:{0} was updated", id);
        return Ok(result.Value);
    }

    [HttpPut("/{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromServices] UpdateSocialNetworksHandler handler,
        [FromServices] IValidator<UpdateSocialNetworksRequest> validator,
        [FromBody] IEnumerable<UpdateSocialNetworksDto> dtos,
        CancellationToken cancellationToken)
    {
        var request = new UpdateSocialNetworksRequest(id, dtos);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        var result = await handler.Handle(request, cancellationToken);
        Log.Information("Volunteer id:{0} was updated", id);
        return Ok(result.Value);
    }

    [HttpPut("/{id:guid}/donation-details")]
    public async Task<ActionResult<Guid>> UpdateDonationDetails(
        [FromRoute] Guid id,
        [FromServices] UpdateDonationDetailsHandler handler,
        [FromServices] IValidator<UpdateDonationDetailsRequest> validator,
        [FromBody] UpdateDonationDetailsDto dto,
        CancellationToken cancellationToken)
    {
        var request = new UpdateDonationDetailsRequest(id, dto);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        var result = await handler.Handle(request, cancellationToken);
        Log.Information("Volunteer id:{0} was updated", id);
        return Ok(result.Value);
    }
}