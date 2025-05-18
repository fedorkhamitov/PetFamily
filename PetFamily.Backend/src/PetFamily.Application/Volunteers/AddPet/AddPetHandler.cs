using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Modules;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler(
    IVolunteersRepository volunteersRepository,
    ISpeciesRepository speciesRepository,
    ILogger<AddPetHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult =
            await volunteersRepository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var address = Address.Create(
            command.Address.ZipCode,
            command.Address.Country,
            command.Address.State,
            command.Address.City,
            command.Address.StreetName,
            command.Address.StreetNumber,
            command.Address.Apartment);

        if (address.IsFailure)
            return address.Error;

        var donationDetails = DonationDetails.Create(command.DonationDetails.Name,
            command.DonationDetails.Description);

        if (donationDetails.IsFailure)
            return donationDetails.Error;

        var position = Position.Create(command.Position);
        if (position.IsFailure)
            return position.Error;

        var isSpeciesAndBreedExists =
            await speciesRepository.IsSpeciesAndBreedExists(command.SpeciesId, command.BreedId, cancellationToken);
        if (isSpeciesAndBreedExists.Value == false)
            return Error.NotFound("species.and.breed.exist", "Not found species or breed by id");

        var petResult = new Pet(
            command.Name,
            command.Description,
            command.IsSterilized,
            command.IsVaccinated,
            new SpeciesInfo(command.SpeciesId, command.BreedId),
            command.Color,
            address.Value,
            command.Weight,
            command.Height,
            command.PhoneNumber,
            command.BirthDate,
            donationDetails.Value,
            position.Value,
            command.HelpStatus);

        volunteerResult.Value.AddPet(petResult);
        await volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        logger.LogInformation("Created new pet id: {0}", petResult.Id);
        return petResult.Id;
    }
}