using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Infrastructure;
using PetFamily.Domain.Models;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly AppDbContext _dbContext;
    public VolunteersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(v => v.Pets)
            .ThenInclude(p => p.SpeciesInfo)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);
        return volunteer;
    }

    public async Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken)
    {
        _dbContext.Attach(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Value;
    }
}