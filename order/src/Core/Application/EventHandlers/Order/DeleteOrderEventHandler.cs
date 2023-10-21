namespace Application.EventHandlers.Order;
public class DeleteOrderEventHandler : EventHandler<DeleteOrder, IOrderState>
{
    public DeleteOrderEventHandler(IOrderState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(DeleteOrder deleteOrder)
    {
        var order = deleteOrder.Get<Domain.Aggregates.Order.Order>();
        var result = Dp.State.Order.Delete(order.ID);
        return result;
    }
}