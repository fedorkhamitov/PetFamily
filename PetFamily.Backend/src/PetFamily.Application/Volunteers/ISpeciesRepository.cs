using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Volunteers;

public interface ISpeciesRepository
{
    Task<Result<bool, Error>> IsSpeciesAndBreedExists(Guid speciesId, Guid breedId, CancellationToken cancellationToken);
}