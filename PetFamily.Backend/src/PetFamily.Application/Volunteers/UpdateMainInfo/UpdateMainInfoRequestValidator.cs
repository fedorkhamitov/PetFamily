using FluentValidation;
using PetFamily.Application.Validation;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoRequestValidator : AbstractValidator<UpdateMainInfoRequest>
{
    public UpdateMainInfoRequestValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithError("Id");
    }
}