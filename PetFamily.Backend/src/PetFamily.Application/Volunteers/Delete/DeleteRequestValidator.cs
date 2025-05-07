using FluentValidation;
using PetFamily.Application.Validation;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteRequestValidator : AbstractValidator<DeleteRequest>
{
    public DeleteRequestValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithError("Id");
    }
}