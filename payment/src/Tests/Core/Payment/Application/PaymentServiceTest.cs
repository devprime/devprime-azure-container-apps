namespace Tests_Application.Payment;
public class PaymentServiceTest
{
    [Fact]
    [Trait("Category", "Add")]
    [Trait("Category", "Success")]

    public void Add_Command_Result()
    {
        //Arrange
        var serviceMock = new PaymentServiceMock();
        var command = serviceMock.MockCommand();
        var service = serviceMock.MockPaymentService();
        //Act
        service.Add(command);
        //Assert
        Assert.NotNull(serviceMock.OutPutDomainEvents[0] as PaymentCreated);
    }
    [Fact]
    [Trait("Category", "Update")]
    [Trait("Category", "Success")]

    public void Update_Command_Result()
    {
        //Arrange
        var serviceMock = new PaymentServiceMock();
        var command = serviceMock.MockCommand();
        var service = serviceMock.MockPaymentService();
        //Act
        service.Update(command);
        //Assert
        Assert.NotNull(serviceMock.OutPutDomainEvents[0] as PaymentUpdated);
    }
    [Fact]
    [Trait("Category", "Delete")]
    [Trait("Category", "Success")]

    public void Delete_Command_Result()
    {
        //Arrange
        var serviceMock = new PaymentServiceMock();
        var command = serviceMock.MockCommand();
        var service = serviceMock.MockPaymentService();
        //Act
        service.Delete(command);
        //Assert
        Assert.NotNull(serviceMock.OutPutDomainEvents[0] as PaymentDeleted);
    }
}