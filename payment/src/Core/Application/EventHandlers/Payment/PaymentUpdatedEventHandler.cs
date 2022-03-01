namespace Application.EventHandlers.Payment;
public class PaymentUpdatedEventHandler : EventHandler<PaymentUpdated, IPaymentState>
{
    public PaymentUpdatedEventHandler(IPaymentState state, IDp dp) : base(state, dp)
    {
    }

    public override dynamic Handle(PaymentUpdated paymentUpdated)
    {
        var success = false;
        var payment = paymentUpdated.Get<Domain.Aggregates.Payment.Payment>();
        Dp.State.Payment.Update(payment);
        var destination = Dp.Settings.Default("stream.paymentevents");
        var eventName = "PaymentUpdated";
        var eventData = new PaymentUpdatedEventDTO()
        {ID = payment.ID, CustomerName = payment.CustomerName, OrderID = payment.OrderID, Value = payment.Value};
        Dp.Stream.Send(destination, eventName, eventData);
        success = true;
        return success;
    }
}