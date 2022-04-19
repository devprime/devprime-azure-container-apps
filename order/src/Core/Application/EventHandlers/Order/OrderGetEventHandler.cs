namespace Application.EventHandlers.Order;
public class OrderGetEventHandler : EventHandler<OrderGet, IOrderState>
{
    public OrderGetEventHandler(IOrderState state, IDp dp) : base(state, dp)
    {
    }

    public override dynamic Handle(OrderGet domainEvent)
    {
        var source = Dp.State.Order.GetAll(domainEvent.Limit, domainEvent.Offset, domainEvent.Ordering, domainEvent.Sort, domainEvent.Filter);
        var total = Dp.State.Order.Total(domainEvent.Filter);
        return (source, total);
    }
}