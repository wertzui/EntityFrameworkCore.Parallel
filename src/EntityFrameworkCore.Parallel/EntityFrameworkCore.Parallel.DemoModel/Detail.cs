namespace EntityFrameworkCore.Parallel.DemoModel;

public class Detail : EntityBase
{
    public string? Product { get; set; }
    public int Quantity { get; set; }
    public long OrderId { get; set; }
    public virtual Order? Order { get; set; }
}