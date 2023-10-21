namespace DevPrime.State.Repositories.Order.Model;
public class Item
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId _Id { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid ID { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
    public string SKU { get; set; }
    public double Price { get; set; }
}