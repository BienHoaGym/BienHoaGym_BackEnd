namespace Gym.Domain.Common;

public abstract class BaseEntity
{
    // Đổi từ int sang Guid
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    public void CheckId()
    {
        if (Id == Guid.Empty) Id = Guid.NewGuid();
    }
}
