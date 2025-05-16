using System.Text.RegularExpressions;
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetRequestValidator : AbstractValidator<AddPetRequest>
{
    public AddPetRequestValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithError("Name");

        RuleFor(c => c.Description).NotEmpty().WithError("Description");

        RuleFor(c => c.IsSterilized).NotEmpty().WithError("IsSterilized");
        
        RuleFor(c => c.IsVaccinated).NotEmpty().WithError("IsVaccinated");

        RuleFor(c => c.SpeciesId).NotEmpty().WithError("SpeciesId");

        RuleFor(c => c.BreedId).NotEmpty().WithError("BreedId");

        RuleFor(c => c.Address)
            .MustBeValueObject(a => 
                Address.Create(
                    a.ZipCode, 
                    a.Country, 
                    a.State,
                    a.City,
                    a.StreetName,
                    a.StreetNumber,
                    a.Apartment));

        RuleFor(c => c.Color).NotEmpty().WithError("Color");

        RuleFor(c => c.Weight)
            .NotEmpty()
            .LessThan(Constants.MAX_PET_WIGHT)
            .GreaterThan(Constants.MIN_PET_WIGHT)
            .WithError("Weight");

        RuleFor(c => c.Height)
            .NotEmpty()
            .LessThan(Constants.MAX_PET_HEIGHT)
            .GreaterThan(Constants.MIN_PET_HEIGHT)
            .WithError("Height");

        RuleFor(c => c.PhoneNumber)
            .Must(pn => pn.Length == 10 && Regex.IsMatch(pn, @"\d+$"))
            .WithError("PhoneNumber");

        RuleFor(c => c.BirthDate)
            .NotEmpty()
            .WithError("BirthDate");

        RuleFor(c => c.DonationDetails)
            .MustBeValueObject(dd => DonationDetails.Create(dd.Name, dd.Description));
    }
}