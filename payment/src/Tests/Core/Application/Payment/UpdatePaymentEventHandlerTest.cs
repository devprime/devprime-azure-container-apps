namespace Core.Tests;
public class UpdatePaymentEventHandlerTest
{
    public UpdatePayment Create_Payment_Object_OK(DpTest dpTest)
    {
        var payment = PaymentTest.Create_Payment_Required_Properties_OK(dpTest);
        var updatePayment = new UpdatePayment();
        dpTest.SetDomainEventObject(updatePayment, payment);
        return updatePayment;
    }
    [Fact]
    [Trait("EventHandler", "UpdatePaymentEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_PaymentObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        object parameter = null;
        var updatePayment = Create_Payment_Object_OK(dpTest);
        var payment = dpTest.GetDomainEventObject<Domain.Aggregates.Payment.Payment>(updatePayment);
        var repositoryMock = new Mock<IPaymentRepository>();
        repositoryMock.Setup((o) => o.Update(payment)).Returns(true).Callback(() =>
        {
            parameter = payment;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IPaymentState>();
        stateMock.SetupGet((o) => o.Payment).Returns(repository);
        var state = stateMock.Object;
        var updatePaymentEventHandler = new Application.EventHandlers.Payment.UpdatePaymentEventHandler(state, dpTest.MockDp<IPaymentState>(state));
        //Act
        var result = updatePaymentEventHandler.Handle(updatePayment);
        //Assert
        Assert.Equal(parameter, payment);
        Assert.Equal(result, true);
    }
}