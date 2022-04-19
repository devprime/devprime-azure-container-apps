namespace Application.EventHandlers;
public class EventHandler : IEventHandler
{
    public EventHandler(IHandler handler)
    {
        handler.Add<OrderCreated, OrderCreatedEventHandler>();
        handler.Add<OrderDeleted, OrderDeletedEventHandler>();
        handler.Add<OrderGet, OrderGetEventHandler>();
        handler.Add<OrderUpdated, OrderUpdatedEventHandler>();
    }
}