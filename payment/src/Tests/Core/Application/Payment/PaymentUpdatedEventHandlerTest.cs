namespace Core.Tests;
public class PaymentUpdatedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.paymentevents", "paymentevents");
        return settings;
    }
    private PaymentUpdatedEventDTO SetEventData(Domain.Aggregates.Payment.Payment payment)
    {
        return new PaymentUpdatedEventDTO()
        {ID = payment.ID, CustomerName = payment.CustomerName, OrderID = payment.OrderID, Value = payment.Value};
    }
    public PaymentUpdated Create_Payment_Object_OK(DpTest dpTest)
    {
        var payment = PaymentTest.Create_Payment_Required_Properties_OK(dpTest);
        var paymentUpdated = new PaymentUpdated();
        dpTest.SetDomainEventObject(paymentUpdated, payment);
        return paymentUpdated;
    }
    [Fact]
    [Trait("EventHandler", "PaymentUpdatedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_PaymentObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var settings = CustomSettings();
        var paymentUpdated = Create_Payment_Object_OK(dpTest);
        var payment = dpTest.GetDomainEventObject<Domain.Aggregates.Payment.Payment>(paymentUpdated);
        var paymentUpdatedEventHandler = new Application.EventHandlers.Payment.PaymentUpdatedEventHandler(null, dpTest.MockDp<IPaymentState>(null));
        dpTest.SetupSettings(paymentUpdatedEventHandler.Dp, settings);
        dpTest.SetupStream(paymentUpdatedEventHandler.Dp);
        //Act
        var result = paymentUpdatedEventHandler.Handle(paymentUpdated);
        //Assert
        var sentEvents = dpTest.GetSentEvents(paymentUpdatedEventHandler.Dp);
        var paymentUpdatedEventDTO = SetEventData(payment);
        Assert.Equal(sentEvents[0].Destination, settings["stream.paymentevents"]);
        Assert.Equal("PaymentUpdated", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, paymentUpdatedEventDTO);
        Assert.Equal(result, true);
    }
}