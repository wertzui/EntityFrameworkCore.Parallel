namespace EntityFrameworkCore.Parallel.DemoModel;

public abstract class EntityBase
{
    public long Id { get; set; }
    public byte[]? Timestamp { get; set; }
}
