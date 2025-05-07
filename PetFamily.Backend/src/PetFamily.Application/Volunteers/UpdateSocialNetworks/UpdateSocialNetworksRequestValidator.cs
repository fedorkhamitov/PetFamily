using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public class UpdateSocialNetworksRequestValidator : AbstractValidator<UpdateSocialNetworksRequest>
{
    public UpdateSocialNetworksRequestValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithError("Id");
        RuleForEach(r => r.Dtos)
            .MustBeValueObject(s => SocialNetwork.Create(s.Name, s.Url));
    }
}