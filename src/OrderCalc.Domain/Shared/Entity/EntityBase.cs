namespace OrderCalc.Domain.Shared.Entity;

public abstract class EntityBase
{
    public int Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public void SetCreated(int id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}