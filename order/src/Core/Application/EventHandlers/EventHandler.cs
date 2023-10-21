namespace Application.EventHandlers;
public class EventHandler : IEventHandler
{
    public EventHandler(IHandler handler)
    {
        handler.Add<CreateOrder, CreateOrderEventHandler>();
        handler.Add<DeleteOrder, DeleteOrderEventHandler>();
        handler.Add<OrderCreated, OrderCreatedEventHandler>();
        handler.Add<OrderDeleted, OrderDeletedEventHandler>();
        handler.Add<OrderGetByID, OrderGetByIDEventHandler>();
        handler.Add<OrderGet, OrderGetEventHandler>();
        handler.Add<OrderUpdated, OrderUpdatedEventHandler>();
        handler.Add<UpdateOrder, UpdateOrderEventHandler>();
    }
}