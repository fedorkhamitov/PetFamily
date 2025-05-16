using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private AppDbContext _dbContext;

    public SpeciesRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Result<bool, Error>> IsSpeciesAndBreedExists(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbContext.Species
                .AnyAsync(s => s.Id == speciesId && s.Breeds
                    .Any(b => b.Id == breedId), cancellationToken: cancellationToken);
            return result;
        }
        catch (Exception e)
        {
            return Error.Failure("fail.context", e.Message);
        }
    }
}