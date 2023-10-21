namespace Application.EventHandlers.Order;
public class OrderGetByIDEventHandler : EventHandler<OrderGetByID, IOrderState>
{
    public OrderGetByIDEventHandler(IOrderState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(OrderGetByID orderGetByID)
    {
        var order = orderGetByID.Get<Domain.Aggregates.Order.Order>();
        var result = Dp.State.Order.Get(order.ID);
        return result;
    }
}