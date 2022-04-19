namespace Tests_Application;
public class OrderStateMock : IOrderState
{
    public IOrderRepository Order { get; set; }
    public OrderStateMock()
    {
    }

    public OrderStateMock(IOrderRepository order)
    {
        Order = order;
    }
}