namespace Application.EventHandlers.Payment;
public class PaymentGetByIDEventHandler : EventHandler<PaymentGetByID, IPaymentState>
{
    public PaymentGetByIDEventHandler(IPaymentState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(PaymentGetByID paymentGetByID)
    {
        var payment = paymentGetByID.Get<Domain.Aggregates.Payment.Payment>();
        var result = Dp.State.Payment.Get(payment.ID);
        return result;
    }
}