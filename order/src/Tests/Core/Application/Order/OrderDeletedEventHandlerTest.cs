namespace Core.Tests;
public class OrderDeletedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.orderevents", "orderevents");
        return settings;
    }
    private OrderDeletedEventDTO SetEventData(Domain.Aggregates.Order.Order order)
    {
        return new OrderDeletedEventDTO()
        {ID = order.ID, CustomerName = order.CustomerName, CustomerTaxID = order.CustomerTaxID, Total = order.Total};
    }
    public OrderDeleted Create_Order_Object_OK(DpTest dpTest)
    {
        var order = OrderTest.Create_Order_Required_Properties_OK(dpTest);
        var orderDeleted = new OrderDeleted();
        dpTest.SetDomainEventObject(orderDeleted, order);
        return orderDeleted;
    }
    [Fact]
    [Trait("EventHandler", "OrderDeletedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_OrderObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var settings = CustomSettings();
        var orderDeleted = Create_Order_Object_OK(dpTest);
        var order = dpTest.GetDomainEventObject<Domain.Aggregates.Order.Order>(orderDeleted);
        var orderDeletedEventHandler = new Application.EventHandlers.Order.OrderDeletedEventHandler(null, dpTest.MockDp<IOrderState>(null));
        dpTest.SetupSettings(orderDeletedEventHandler.Dp, settings);
        dpTest.SetupStream(orderDeletedEventHandler.Dp);
        //Act
        var result = orderDeletedEventHandler.Handle(orderDeleted);
        //Assert
        var sentEvents = dpTest.GetSentEvents(orderDeletedEventHandler.Dp);
        var orderDeletedEventDTO = SetEventData(order);
        Assert.Equal(sentEvents[0].Destination, settings["stream.orderevents"]);
        Assert.Equal("OrderDeleted", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, orderDeletedEventDTO);
        Assert.Equal(result, true);
    }
}