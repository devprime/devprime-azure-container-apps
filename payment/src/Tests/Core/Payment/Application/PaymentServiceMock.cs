namespace Tests_Application.Payment;
public class PaymentModelMock : Application.Services.Payment.Model.Payment
{
    public override Domain.Aggregates.Payment.Payment ToDomain()
    {
        var agg = MockPaymentAggRoot(this.ID, this.CustomerName, this.OrderID, this.Value);
        return agg;
    }

    public override PaymentModelMock ToPayment(Domain.Aggregates.Payment.Payment agg)
    {
        var model = new PaymentModelMock();
        model.ID = agg.ID;
        model.CustomerName = agg.CustomerName;
        model.OrderID = agg.OrderID;
        model.Value = agg.Value;
        return model;
    }

    public override Domain.Aggregates.Payment.Payment ToDomain(Guid id)
    {
        var agg = MockPaymentAggRoot(Guid.Empty, String.Empty, Guid.Empty, 0);
        agg.DevPrimeSetProperty<Guid>("ID", id);
        return agg;
    }
    private Domain.Aggregates.Payment.Payment MockPaymentAggRoot(Guid iD, string customerName, Guid orderID, double value)
    {
        var AggMock = new Mock<Domain.Aggregates.Payment.Payment>(iD, customerName, orderID, value);
        var agg = AggMock.Object;
        AggMock.Setup(o => o.Add()).Callback(() =>
        {
            agg.Dp.ProcessEvent(new PaymentCreated());
        });
        AggMock.Setup(o => o.Update()).Callback(() =>
        {
            agg.Dp.ProcessEvent(new PaymentUpdated());
        });
        AggMock.Setup(o => o.Delete()).Callback(() =>
        {
            agg.Dp.ProcessEvent(new PaymentDeleted());
        });
        return agg;
    }
}
public class PaymentServiceMock
{
    public List<DomainEvent> OutPutDomainEvents { get; set; }
    public Application.Services.Payment.Model.Payment MockCommand()
    {
        var agg = new PaymentModelMock();
        agg.CustomerName = Faker.Lorem.Sentence();
        agg.Value = Faker.RandomNumber.Next();
        return agg;
    }

    public PaymentService MockPaymentService()
    {
        OutPutDomainEvents = new List<DomainEvent>();
        var state = new PaymentStateMock();
        var dp = new DpMock<IPaymentState>(state, (domainevent) =>
        {
            OutPutDomainEvents.Add(domainevent);
        });
        var srv = new PaymentService(state, dp);
        return srv;
    }
}