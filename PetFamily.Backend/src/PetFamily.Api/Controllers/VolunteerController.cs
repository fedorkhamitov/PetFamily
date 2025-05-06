using System.Runtime.InteropServices.JavaScript;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;
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
        if (validationResult.IsValid == false)
        {
            var validationErrors = validationResult.Errors;
            
            var responseErrors = from validationError in validationErrors
                let errorMessage = validationError.ErrorMessage
                let error = Error.Deserialize(errorMessage)
                select new ResponseError(error.Code, error.Message, validationError.PropertyName);
            
            var envelope = Envelope.Error(responseErrors);
            return BadRequest(envelope);
        }

        var result = await handler.Handle(request, cancellationToken);
        Log.Information("Created volunteer {0}", result.Value);
        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
}