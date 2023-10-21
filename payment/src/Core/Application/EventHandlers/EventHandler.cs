namespace Application.EventHandlers;
public class EventHandler : IEventHandler
{
    public EventHandler(IHandler handler)
    {
        handler.Add<CreatePayment, CreatePaymentEventHandler>();
        handler.Add<DeletePayment, DeletePaymentEventHandler>();
        handler.Add<PaymentCreated, PaymentCreatedEventHandler>();
        handler.Add<PaymentDeleted, PaymentDeletedEventHandler>();
        handler.Add<PaymentGetByID, PaymentGetByIDEventHandler>();
        handler.Add<PaymentGet, PaymentGetEventHandler>();
        handler.Add<PaymentUpdated, PaymentUpdatedEventHandler>();
        handler.Add<UpdatePayment, UpdatePaymentEventHandler>();
    }
}