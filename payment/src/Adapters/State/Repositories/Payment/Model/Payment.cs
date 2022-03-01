namespace DevPrime.State.Repositories.Payment.Model;
public class Payment
{
    [BsonId]
    [BsonElement("_id")]

    public ObjectId Id { get; set; }
    [BsonRepresentation(BsonType.String)]

    public Guid PaymentID { get; set; }
    public string CustomerName { get; set; }
    [BsonRepresentation(BsonType.String)]

    public Guid OrderID { get; set; }
    public double Value { get; set; }
}