namespace DevPrime.State.States;
public class PaymentState : IPaymentState
{
    public IPaymentRepository Payment { get; set; }
    public PaymentState(IPaymentRepository payment)
    {
        Payment = payment;
    }
}