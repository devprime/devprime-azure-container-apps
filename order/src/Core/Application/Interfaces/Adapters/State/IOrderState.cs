namespace Application.Interfaces.Adapters.State;
public interface IOrderState
{
    IOrderRepository Order { get; set; }
}