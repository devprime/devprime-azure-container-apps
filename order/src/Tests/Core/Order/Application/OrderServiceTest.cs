namespace Tests_Application.Order;
public class OrderServiceTest
{
    [Fact]
    [Trait("Category", "Add")]
    [Trait("Category", "Success")]

    public void Add_Command_Result()
    {
        //Arrange
        var serviceMock = new OrderServiceMock();
        var command = serviceMock.MockCommand();
        var service = serviceMock.MockOrderService();
        //Act
        service.Add(command);
        //Assert
        Assert.NotNull(serviceMock.OutPutDomainEvents[0] as OrderCreated);
    }
    [Fact]
    [Trait("Category", "Update")]
    [Trait("Category", "Success")]

    public void Update_Command_Result()
    {
        //Arrange
        var serviceMock = new OrderServiceMock();
        var command = serviceMock.MockCommand();
        var service = serviceMock.MockOrderService();
        //Act
        service.Update(command);
        //Assert
        Assert.NotNull(serviceMock.OutPutDomainEvents[0] as OrderUpdated);
    }
    [Fact]
    [Trait("Category", "Delete")]
    [Trait("Category", "Success")]

    public void Delete_Command_Result()
    {
        //Arrange
        var serviceMock = new OrderServiceMock();
        var command = serviceMock.MockCommand();
        var service = serviceMock.MockOrderService();
        //Act
        service.Delete(command);
        //Assert
        Assert.NotNull(serviceMock.OutPutDomainEvents[0] as OrderDeleted);
    }
}