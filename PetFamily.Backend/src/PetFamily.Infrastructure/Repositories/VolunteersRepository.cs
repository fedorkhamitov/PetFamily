using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;

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
            .SingleAsync(v => v.Id == volunteerId, cancellationToken);
        
        return volunteer;
    }

    public async Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken)
    {
        _dbContext.Volunteers.Attach(volunteer);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }

    public async Task<Guid> HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Remove(volunteer);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }
}