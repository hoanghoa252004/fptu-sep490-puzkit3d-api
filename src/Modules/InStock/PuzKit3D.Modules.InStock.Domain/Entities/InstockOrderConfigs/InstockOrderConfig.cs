namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderConfigs;

public class InstockOrderConfig
{
    public Guid Id { get; private set; }
    public int OrderMustCompleteInDays { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InstockOrderConfig() { }

    public InstockOrderConfig(int orderMustCompleteInDays)
    {
        Id = Guid.NewGuid();
        OrderMustCompleteInDays = orderMustCompleteInDays;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(int orderMustCompleteInDays)
    {
        OrderMustCompleteInDays = orderMustCompleteInDays;
        UpdatedAt = DateTime.UtcNow;
    }
}
