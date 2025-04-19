namespace OrderCalc.Domain.Common;

public abstract class EntityBase
{
    public int Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public void SetCreate()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}