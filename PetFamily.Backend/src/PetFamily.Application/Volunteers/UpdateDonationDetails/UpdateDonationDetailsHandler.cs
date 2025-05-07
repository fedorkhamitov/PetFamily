using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Infrastructure;

namespace PetFamily.Application.Volunteers.UpdateDonationDetails;

public class UpdateDonationDetailsHandler(IVolunteersRepository volunteersRepository,
    ILogger<UpdateDonationDetailsHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        UpdateDonationDetailsRequest request,
        CancellationToken cancellationToken)
    {
        var volunteer = await volunteersRepository
            .GetById(VolunteerId.Create(request.Id), cancellationToken);
        if (volunteer.IsFailure)
            return Error.Failure(volunteer.Error.Code, volunteer.Error.Message);
        var donationDetails = DonationDetails.Create(request.Dto.Name, request.Dto.Description);
        volunteer.Value.UpdateDonationDetails(donationDetails.Value);
        await volunteersRepository.Save(volunteer.Value, cancellationToken);
        logger.LogInformation("Updated donation details for voluteer id: {0}, name: {1},{2}, phone: {3}",
            volunteer.Value.Id, volunteer.Value.Name.FirstName, volunteer.Value.Name.LastName, volunteer.Value.PhoneNumber);
        return volunteer.Value.Id.Value;
    }
}