namespace DevPrime.State.States;
public class OrderState : IOrderState
{
    public IOrderRepository Order { get; set; }
    public OrderState(IOrderRepository order)
    {
        Order = order;
    }
}