namespace Core.Tests;
public class DeleteOrderEventHandlerTest
{
    public DeleteOrder Create_Order_Object_OK(DpTest dpTest)
    {
        var order = OrderTest.Create_Order_Required_Properties_OK(dpTest);
        var deleteOrder = new DeleteOrder();
        dpTest.SetDomainEventObject(deleteOrder, order);
        return deleteOrder;
    }
    [Fact]
    [Trait("EventHandler", "DeleteOrderEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_OrderObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        object parameter = null;
        var deleteOrder = Create_Order_Object_OK(dpTest);
        var order = dpTest.GetDomainEventObject<Domain.Aggregates.Order.Order>(deleteOrder);
        var repositoryMock = new Mock<IOrderRepository>();
        repositoryMock.Setup((o) => o.Delete(order.ID)).Returns(true).Callback(() =>
        {
            parameter = order;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IOrderState>();
        stateMock.SetupGet((o) => o.Order).Returns(repository);
        var state = stateMock.Object;
        var deleteOrderEventHandler = new Application.EventHandlers.Order.DeleteOrderEventHandler(state, dpTest.MockDp<IOrderState>(state));
        //Act
        var result = deleteOrderEventHandler.Handle(deleteOrder);
        //Assert
        Assert.Equal(parameter, order);
        Assert.Equal(result, true);
    }
}