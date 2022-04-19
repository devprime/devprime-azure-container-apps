namespace Tests_Application;
public class PaymentStateMock : IPaymentState
{
    public IPaymentRepository Payment { get; set; }
    public PaymentStateMock()
    {
    }

    public PaymentStateMock(IPaymentRepository payment)
    {
        Payment = payment;
    }
}