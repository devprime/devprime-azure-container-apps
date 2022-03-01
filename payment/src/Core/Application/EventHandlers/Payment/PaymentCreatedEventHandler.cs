namespace Application.EventHandlers.Payment;
public class PaymentCreatedEventHandler : EventHandler<PaymentCreated, IPaymentState>
{
    public PaymentCreatedEventHandler(IPaymentState state, IDp dp) : base(state, dp)
    {
    }

    public override dynamic Handle(PaymentCreated paymentCreated)
    {
        var success = false;
        var payment = paymentCreated.Get<Domain.Aggregates.Payment.Payment>();
        Dp.State.Payment.Add(payment);
        var destination = Dp.Settings.Default("stream.paymentevents");
        var eventName = "PaymentCreated";
        var eventData = new PaymentCreatedEventDTO()
        {ID = payment.ID, CustomerName = payment.CustomerName, OrderID = payment.OrderID, Value = payment.Value};
        Dp.Stream.Send(destination, eventName, eventData);
        success = true;
        return success;
    }
}