using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoDtoValidator : AbstractValidator<UpdateMainInfoDto>
{
    public UpdateMainInfoDtoValidator()
    {
        RuleFor(r => r.Name).MustBeValueObject(h =>
            HumanName.Create(h.FirstName, h.SecondName, h.LastName));
        RuleFor(r => r.Description).NotEmpty().WithError("Description");
        RuleFor(r => r.YearsOfWorkExp).LessThan((ushort)Constants.MAX_LOW_TEXT_LENGHT)
            .WithError("YearsOfWorkExp");
        RuleFor(r => r.Email).EmailAddress().WithError("Email");
        RuleFor(r => r.PhoneNumber).NotEmpty().WithError("PhoneNumber");
    }
}