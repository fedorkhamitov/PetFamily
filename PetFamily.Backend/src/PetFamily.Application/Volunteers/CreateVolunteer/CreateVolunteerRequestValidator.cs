using System.Data;
using System.Text.RegularExpressions;
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.VolunteerName).MustBeValueObject(h => 
            HumanName.Create(h.FirstName, h.SecondName, h.LastName));
        RuleFor(c => c.Email).EmailAddress()
            .WithError("Email");
        RuleFor(c => c.Description).NotEmpty().WithError("Description");
        RuleFor(c => c.Description.Length).LessThan(Constants.MAX_HIGH_TEXT_LENGHT)
            .WithError("Description.Length");
        RuleFor(c => c.YearsOfWorkExp).LessThan((ushort)Constants.MAX_LOW_TEXT_LENGHT)
            .WithError("YearsOfWorkExp");
        RuleFor(c => c.PhoneNumber)
            .Must(pn => pn.Length == 10 && !Regex.IsMatch(pn, @"\d+$"))
            .WithError("PhoneNumber");
        RuleFor(c => c.DonationDetails)
            .MustBeValueObject(d => DonationDetails.Create(d.Name, d.Description));
        RuleForEach(c => c.SocialNetworks).MustBeValueObject(s =>
            SocialNetwork.Create(s.Name, s.Url));
    }
}