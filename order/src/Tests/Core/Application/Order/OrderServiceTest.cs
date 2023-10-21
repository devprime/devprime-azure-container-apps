namespace Core.Tests;
public class OrderServiceTest
{
    public Application.Services.Order.Model.Order SetupCommand(Action add, Action update, Action delete, DpTest dpTest)
    {
        var domainOrderMock = new Mock<Domain.Aggregates.Order.Order>();
        domainOrderMock.Setup((o) => o.Add()).Callback(add);
        domainOrderMock.Setup((o) => o.Update()).Callback(update);
        domainOrderMock.Setup((o) => o.Delete()).Callback(delete);
        var order = domainOrderMock.Object;
        dpTest.MockDpDomain(order);
        dpTest.Set<string>(order, "CustomerName", Faker.Lorem.Sentence(1));
        dpTest.Set<string>(order, "CustomerTaxID", Faker.Lorem.Sentence(1));
        var applicationOrderMock = new Mock<Application.Services.Order.Model.Order>();
        applicationOrderMock.Setup((o) => o.ToDomain()).Returns(order);
        var applicationOrder = applicationOrderMock.Object;
        return applicationOrder;
    }
    public IOrderService SetupApplicationService(DpTest dpTest)
    {
        var state = new Mock<IOrderState>().Object;
        var orderService = new Application.Services.Order.OrderService(state, dpTest.MockDp());
        return orderService;
    }
    [Fact]
    [Trait("ApplicationService", "OrderService")]
    [Trait("ApplicationService", "Success")]
    public void Add_CommandNotNull_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var addCalled = false;
        var add = () =>
        {
            addCalled = true;
        };
        var command = SetupCommand(add, () =>
        {
        }, () =>
        {
        }, dpTest);
        var orderService = SetupApplicationService(dpTest);
        //Act
        orderService.Add(command);
        //Assert
        Assert.True(addCalled);
    }
    [Fact]
    [Trait("ApplicationService", "OrderService")]
    [Trait("ApplicationService", "Success")]
    public void Update_CommandFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var updateCalled = false;
        var update = () =>
        {
            updateCalled = true;
        };
        var command = SetupCommand(() =>
        {
        }, update, () =>
        {
        }, dpTest);
        var orderService = SetupApplicationService(dpTest);
        //Act
        orderService.Update(command);
        //Assert
        Assert.True(updateCalled);
    }
    [Fact]
    [Trait("ApplicationService", "OrderService")]
    [Trait("ApplicationService", "Success")]
    public void Delete_CommandFilled_Success()
    {
        //Arrange        
        var dpTest = new DpTest();
        var deleteCalled = false;
        var delete = () =>
        {
            deleteCalled = true;
        };
        var command = SetupCommand(() =>
        {
        }, () =>
        {
        }, delete, dpTest);
        var orderService = SetupApplicationService(dpTest);
        //Act
        orderService.Delete(command);
        //Assert
        Assert.True(deleteCalled);
    }
}