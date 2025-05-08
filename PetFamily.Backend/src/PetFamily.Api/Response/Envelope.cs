using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;

namespace PetFamily.Api.Response;

public record Envelope
{
    public object? Result { get; }
    public List<ResponseError> Errors { get; }
    public DateTime TimeGenerated { get; }

    public Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        TimeGenerated = DateTime.UtcNow;
    }

    public static Envelope Ok(object? result = null) => new(result,[]);
    public static Envelope Error(IEnumerable<ResponseError> errors) => new(null, errors);
}