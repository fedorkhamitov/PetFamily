using CSharpFunctionalExtensions;
using PetFamily.Domain.Infrastructure;
using PetFamily.Domain.Models;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetById(VolunteerId volunteerId);
}