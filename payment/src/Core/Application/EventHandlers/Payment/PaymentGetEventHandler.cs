namespace Application.EventHandlers.Payment;
public class PaymentGetEventHandler : EventHandler<PaymentGet, IPaymentState>
{
    public PaymentGetEventHandler(IPaymentState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(PaymentGet domainEvent)
    {
        var source = Dp.State.Payment.GetAll(domainEvent.Limit, domainEvent.Offset, domainEvent.Ordering, domainEvent.Sort, domainEvent.Filter);
        var total = Dp.State.Payment.Total(domainEvent.Filter);
        return (source, total);
    }
}