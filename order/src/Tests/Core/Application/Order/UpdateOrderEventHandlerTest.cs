namespace Core.Tests;
public class UpdateOrderEventHandlerTest
{
    public UpdateOrder Create_Order_Object_OK(DpTest dpTest)
    {
        var order = OrderTest.Create_Order_Required_Properties_OK(dpTest);
        var updateOrder = new UpdateOrder();
        dpTest.SetDomainEventObject(updateOrder, order);
        return updateOrder;
    }
    [Fact]
    [Trait("EventHandler", "UpdateOrderEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_OrderObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        object parameter = null;
        var updateOrder = Create_Order_Object_OK(dpTest);
        var order = dpTest.GetDomainEventObject<Domain.Aggregates.Order.Order>(updateOrder);
        var repositoryMock = new Mock<IOrderRepository>();
        repositoryMock.Setup((o) => o.Update(order)).Returns(true).Callback(() =>
        {
            parameter = order;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IOrderState>();
        stateMock.SetupGet((o) => o.Order).Returns(repository);
        var state = stateMock.Object;
        var updateOrderEventHandler = new Application.EventHandlers.Order.UpdateOrderEventHandler(state, dpTest.MockDp<IOrderState>(state));
        //Act
        var result = updateOrderEventHandler.Handle(updateOrder);
        //Assert
        Assert.Equal(parameter, order);
        Assert.Equal(result, true);
    }
}