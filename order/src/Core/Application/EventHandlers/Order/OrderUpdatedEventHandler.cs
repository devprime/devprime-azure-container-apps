namespace Application.EventHandlers.Order;
public class OrderUpdatedEventHandler : EventHandler<OrderUpdated, IOrderState>
{
    public OrderUpdatedEventHandler(IOrderState state, IDp dp) : base(state, dp)
    {
    }

    public override dynamic Handle(OrderUpdated orderUpdated)
    {
        var success = false;
        var order = orderUpdated.Get<Domain.Aggregates.Order.Order>();
        Dp.State.Order.Update(order);
        var destination = Dp.Settings.Default("stream.orderevents");
        var eventName = "OrderUpdated";
        var eventData = new OrderUpdatedEventDTO()
        {ID = order.ID, CustomerName = order.CustomerName, CustomerTaxID = order.CustomerTaxID, Total = order.Total};
        Dp.Stream.Send(destination, eventName, eventData);
        success = true;
        return success;
    }
}