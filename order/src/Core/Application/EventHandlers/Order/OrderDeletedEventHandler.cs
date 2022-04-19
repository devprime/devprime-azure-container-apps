namespace Application.EventHandlers.Order;
public class OrderDeletedEventHandler : EventHandler<OrderDeleted, IOrderState>
{
    public OrderDeletedEventHandler(IOrderState state, IDp dp) : base(state, dp)
    {
    }

    public override dynamic Handle(OrderDeleted orderDeleted)
    {
        var success = false;
        var order = orderDeleted.Get<Domain.Aggregates.Order.Order>();
        Dp.State.Order.Delete(order.ID);
        var destination = Dp.Settings.Default("stream.orderevents");
        var eventName = "OrderDeleted";
        var eventData = new OrderDeletedEventDTO()
        {ID = order.ID, CustomerName = order.CustomerName, CustomerTaxID = order.CustomerTaxID, Total = order.Total};
        Dp.Stream.Send(destination, eventName, eventData);
        success = true;
        return success;
    }
}