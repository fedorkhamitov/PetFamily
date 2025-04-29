using System.Runtime.InteropServices.JavaScript;

namespace PetFamily.Domain.Infrastructure;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            return Error.Validation("value.is.invalid", $"{name ?? "value"} is invalid");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var label = id == null ? "" : $" for id '{id}'";
            return Error.NotFound("record.not.found", $"Record not found{label}");
        }
        
        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? " " : $" {name} ";
            return Error.Validation("lenght.is.invalid", $"invalid{label}lenght");
        }
    }
}