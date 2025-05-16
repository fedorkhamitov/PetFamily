using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;

namespace PetFamily.Infrastructure;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Volunteer> Volunteers { get; init; }
    
    public DbSet<Species> Species { get; init; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
    }

    private ILoggerFactory CreateLoggerFactory() => 
        LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
}