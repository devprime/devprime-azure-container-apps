namespace Core.Tests;
public class PaymentDeletedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.paymentevents", "paymentevents");
        return settings;
    }
    private PaymentDeletedEventDTO SetEventData(Domain.Aggregates.Payment.Payment payment)
    {
        return new PaymentDeletedEventDTO()
        {ID = payment.ID, CustomerName = payment.CustomerName, OrderID = payment.OrderID, Value = payment.Value};
    }
    public PaymentDeleted Create_Payment_Object_OK(DpTest dpTest)
    {
        var payment = PaymentTest.Create_Payment_Required_Properties_OK(dpTest);
        var paymentDeleted = new PaymentDeleted();
        dpTest.SetDomainEventObject(paymentDeleted, payment);
        return paymentDeleted;
    }
    [Fact]
    [Trait("EventHandler", "PaymentDeletedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_PaymentObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var settings = CustomSettings();
        var paymentDeleted = Create_Payment_Object_OK(dpTest);
        var payment = dpTest.GetDomainEventObject<Domain.Aggregates.Payment.Payment>(paymentDeleted);
        var paymentDeletedEventHandler = new Application.EventHandlers.Payment.PaymentDeletedEventHandler(null, dpTest.MockDp<IPaymentState>(null));
        dpTest.SetupSettings(paymentDeletedEventHandler.Dp, settings);
        dpTest.SetupStream(paymentDeletedEventHandler.Dp);
        //Act
        var result = paymentDeletedEventHandler.Handle(paymentDeleted);
        //Assert
        var sentEvents = dpTest.GetSentEvents(paymentDeletedEventHandler.Dp);
        var paymentDeletedEventDTO = SetEventData(payment);
        Assert.Equal(sentEvents[0].Destination, settings["stream.paymentevents"]);
        Assert.Equal("PaymentDeleted", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, paymentDeletedEventDTO);
        Assert.Equal(result, true);
    }
}