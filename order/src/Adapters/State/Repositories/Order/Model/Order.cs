namespace DevPrime.State.Repositories.Order.Model;
public class Order
{
    [BsonId]
    [BsonElement("_id")]

    public ObjectId Id { get; set; }
    [BsonRepresentation(BsonType.String)]

    public Guid OrderID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerTaxID { get; set; }
    public IList<DevPrime.State.Repositories.Order.Model.Item> Itens { get; set; }
    public double Total { get; set; }
}