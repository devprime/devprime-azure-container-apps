namespace Application.Interfaces.Adapters.State;
public interface IPaymentState
{
    IPaymentRepository Payment { get; set; }
}