using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Volunteers;

public interface IBreedRepository
{
    Task<Result<Breed, Error>> GetById(Guid id, CancellationToken cancellationToken);
}