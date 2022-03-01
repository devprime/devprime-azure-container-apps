namespace Application.EventHandlers.Payment;
public class PaymentDeletedEventHandler : EventHandler<PaymentDeleted, IPaymentState>
{
    public PaymentDeletedEventHandler(IPaymentState state, IDp dp) : base(state, dp)
    {
    }

    public override dynamic Handle(PaymentDeleted paymentDeleted)
    {
        var success = false;
        var payment = paymentDeleted.Get<Domain.Aggregates.Payment.Payment>();
        Dp.State.Payment.Delete(payment.ID);
        var destination = Dp.Settings.Default("stream.paymentevents");
        var eventName = "PaymentDeleted";
        var eventData = new PaymentDeletedEventDTO()
        {ID = payment.ID, CustomerName = payment.CustomerName, OrderID = payment.OrderID, Value = payment.Value};
        Dp.Stream.Send(destination, eventName, eventData);
        success = true;
        return success;
    }
}