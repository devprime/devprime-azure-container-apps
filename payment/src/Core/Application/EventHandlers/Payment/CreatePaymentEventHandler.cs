namespace Application.EventHandlers.Payment;
public class CreatePaymentEventHandler : EventHandler<CreatePayment, IPaymentState>
{
    public CreatePaymentEventHandler(IPaymentState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(CreatePayment createPayment)
    {
        var payment = createPayment.Get<Domain.Aggregates.Payment.Payment>();
        var result = Dp.State.Payment.Add(payment);
        return result;
    }
}