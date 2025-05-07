using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.UpdateDonationDetails;

public class UpdateDonationDetailsRequestValidation : AbstractValidator<UpdateDonationDetailsRequest>
{
    public UpdateDonationDetailsRequestValidation()
    {
        RuleFor(r => r.Id).NotEmpty().WithError("Id");
        RuleFor(r => r.Dto)
            .MustBeValueObject(d =>
                DonationDetails.Create(d.Name, d.Description));
    }
}