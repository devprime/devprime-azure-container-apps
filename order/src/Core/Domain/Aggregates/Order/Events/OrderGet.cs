namespace Domain.Aggregates.Order.Events;
public class OrderGet : DomainEvent
{
    public OrderGet() : base()
    {
    }

    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public string Ordering { get; set; }
    public string Filter { get; set; }
    public string Sort { get; set; }
}