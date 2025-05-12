using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities;

public abstract class SoftDeletableEntity : Entity
{
    public bool IsDeleted { get; private set; } = false;
    public DateTime DeletionDate { get; private set; }

    public virtual void Delete()
    {
        DeletionDate = DateTime.UtcNow;
        IsDeleted = true;
    }
    
    public virtual void Restore()
    {
        IsDeleted = false;
    }
}