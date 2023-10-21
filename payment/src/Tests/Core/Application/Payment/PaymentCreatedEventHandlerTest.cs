namespace Core.Tests;
public class PaymentCreatedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.paymentevents", "paymentevents");
        return settings;
    }
    private PaymentCreatedEventDTO SetEventData(Domain.Aggregates.Payment.Payment payment)
    {
        return new PaymentCreatedEventDTO()
        {ID = payment.ID, CustomerName = payment.CustomerName, OrderID = payment.OrderID, Value = payment.Value};
    }
    public PaymentCreated Create_Payment_Object_OK(DpTest dpTest)
    {
        var payment = PaymentTest.Create_Payment_Required_Properties_OK(dpTest);
        var paymentCreated = new PaymentCreated();
        dpTest.SetDomainEventObject(paymentCreated, payment);
        return paymentCreated;
    }
    [Fact]
    [Trait("EventHandler", "PaymentCreatedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_PaymentObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var settings = CustomSettings();
        var paymentCreated = Create_Payment_Object_OK(dpTest);
        var payment = dpTest.GetDomainEventObject<Domain.Aggregates.Payment.Payment>(paymentCreated);
        var paymentCreatedEventHandler = new Application.EventHandlers.Payment.PaymentCreatedEventHandler(null, dpTest.MockDp<IPaymentState>(null));
        dpTest.SetupSettings(paymentCreatedEventHandler.Dp, settings);
        dpTest.SetupStream(paymentCreatedEventHandler.Dp);
        //Act
        var result = paymentCreatedEventHandler.Handle(paymentCreated);
        //Assert
        var sentEvents = dpTest.GetSentEvents(paymentCreatedEventHandler.Dp);
        var paymentCreatedEventDTO = SetEventData(payment);
        Assert.Equal(sentEvents[0].Destination, settings["stream.paymentevents"]);
        Assert.Equal("PaymentCreated", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, paymentCreatedEventDTO);
        Assert.Equal(result, true);
    }
}