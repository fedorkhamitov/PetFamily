using PetFamily.Infrastructure;

namespace PetFamily.Application.Services;

public class CleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _cleanupInterval;
    private readonly TimeSpan _entityLifetime;
    private readonly ILogger<CleanupService> _logger;

    public CleanupService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration,
        ILogger<CleanupService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cleanupInterval = TimeSpan.FromHours(24);
        var lifetimeDays = configuration.GetValue<int>("EntityLifetimeDays", 30);
        _entityLifetime = TimeSpan.FromDays(lifetimeDays);
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_cleanupInterval, stoppingToken);

            using var scope = _serviceScopeFactory.CreateScope();
            
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            var expirationDate = DateTime.UtcNow.Add(-_entityLifetime);
                
            var expiredEntities = dbContext.Volunteers
                .Where(e => e.IsDeleted && e.DeletionDate < expirationDate);

            dbContext.Volunteers.RemoveRange(expiredEntities);

            foreach (var entity in expiredEntities)
            {
                _logger.LogInformation("Volunteer id: {0} was permanently deleted 30 days after the soft deletion.", 
                    entity.Id);
            }
            
            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}