namespace Core.Tests;
public class OrderUpdatedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.orderevents", "orderevents");
        return settings;
    }
    private OrderUpdatedEventDTO SetEventData(Domain.Aggregates.Order.Order order)
    {
        return new OrderUpdatedEventDTO()
        {ID = order.ID, CustomerName = order.CustomerName, CustomerTaxID = order.CustomerTaxID, Total = order.Total};
    }
    public OrderUpdated Create_Order_Object_OK(DpTest dpTest)
    {
        var order = OrderTest.Create_Order_Required_Properties_OK(dpTest);
        var orderUpdated = new OrderUpdated();
        dpTest.SetDomainEventObject(orderUpdated, order);
        return orderUpdated;
    }
    [Fact]
    [Trait("EventHandler", "OrderUpdatedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_OrderObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var settings = CustomSettings();
        var orderUpdated = Create_Order_Object_OK(dpTest);
        var order = dpTest.GetDomainEventObject<Domain.Aggregates.Order.Order>(orderUpdated);
        var orderUpdatedEventHandler = new Application.EventHandlers.Order.OrderUpdatedEventHandler(null, dpTest.MockDp<IOrderState>(null));
        dpTest.SetupSettings(orderUpdatedEventHandler.Dp, settings);
        dpTest.SetupStream(orderUpdatedEventHandler.Dp);
        //Act
        var result = orderUpdatedEventHandler.Handle(orderUpdated);
        //Assert
        var sentEvents = dpTest.GetSentEvents(orderUpdatedEventHandler.Dp);
        var orderUpdatedEventDTO = SetEventData(order);
        Assert.Equal(sentEvents[0].Destination, settings["stream.orderevents"]);
        Assert.Equal("OrderUpdated", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, orderUpdatedEventDTO);
        Assert.Equal(result, true);
    }
}