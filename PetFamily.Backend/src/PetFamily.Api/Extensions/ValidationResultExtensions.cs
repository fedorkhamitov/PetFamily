using FluentValidation.Results;
using PetFamily.Api.Response;
using PetFamily.Domain.Share;
using Serilog;

namespace PetFamily.Api.Extensions;

public static class ValidationResultExtensions
{
    public static Envelope ValidationResultErrorEnvelope(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;
        var responseErrors = (from validationError in validationErrors
                let errorMessage = validationError.ErrorMessage
                let error = Error.Deserialize(errorMessage)
                select new ResponseError(error.Code, error.Message, validationError.PropertyName))
            .ToList();
        var envelope = Envelope.Error(responseErrors);
        foreach (var error in responseErrors)
        {
            Log.Error("Error! code: {0}, message: {1}",
                error.ErrorCode,
                error.ErrorMessage);
        }
        return envelope;
    }
}