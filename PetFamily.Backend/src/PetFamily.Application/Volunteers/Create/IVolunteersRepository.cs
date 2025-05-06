using CSharpFunctionalExtensions;
using PetFamily.Domain.Infrastructure;
using PetFamily.Domain.Models;

namespace PetFamily.Application.Volunteers.Create;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
    Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default);
}