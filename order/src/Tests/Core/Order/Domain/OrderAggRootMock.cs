namespace Tests_Domain.Order;
public partial class OrderAggRootTest
{
    public Domain.Aggregates.Order.Order MockOrder()
    {
        var agg = new Domain.Aggregates.Order.Order(Guid.Empty, String.Empty, String.Empty, new List<Domain.Aggregates.Order.Item>(), 0);
        agg.Dp = new DpDomainMock(null);
        agg.DevPrimeSetProperty<Guid>("ID", Guid.NewGuid());
        agg.DevPrimeSetProperty<String>("CustomerName", Faker.Lorem.Sentence());
        agg.DevPrimeSetProperty<String>("CustomerTaxID", Faker.Lorem.Sentence());
        agg.DevPrimeSetProperty<Double>("Total", Faker.RandomNumber.Next());
        agg.Dp.Source = agg;
        return agg;
    }
}