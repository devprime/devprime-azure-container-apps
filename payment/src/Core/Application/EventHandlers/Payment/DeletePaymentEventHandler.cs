namespace Application.EventHandlers.Payment;
public class DeletePaymentEventHandler : EventHandler<DeletePayment, IPaymentState>
{
    public DeletePaymentEventHandler(IPaymentState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(DeletePayment deletePayment)
    {
        var payment = deletePayment.Get<Domain.Aggregates.Payment.Payment>();
        var result = Dp.State.Payment.Delete(payment.ID);
        return result;
    }
}