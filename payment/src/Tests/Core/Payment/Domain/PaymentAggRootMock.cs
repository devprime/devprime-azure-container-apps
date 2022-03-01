namespace Tests_Domain.Payment;
public partial class PaymentAggRootTest
{
    public Domain.Aggregates.Payment.Payment MockPayment()
    {
        var agg = new Domain.Aggregates.Payment.Payment(Guid.Empty, String.Empty, Guid.Empty, 0);
        agg.Dp = new DpDomainMock(null);
        agg.DevPrimeSetProperty<Guid>("ID", Guid.NewGuid());
        agg.DevPrimeSetProperty<String>("CustomerName", Faker.Lorem.Sentence());
        agg.DevPrimeSetProperty<Guid>("OrderID", Guid.NewGuid());
        agg.DevPrimeSetProperty<Double>("Value", Faker.RandomNumber.Next());
        agg.Dp.Source = agg;
        return agg;
    }
}