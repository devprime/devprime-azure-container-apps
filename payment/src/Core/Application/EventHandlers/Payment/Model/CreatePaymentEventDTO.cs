namespace Application.Services.Payment.Model;
public class CreatePaymentEventDTO
{
    public Guid ID { get; set; }
    public string CustomerName { get; set; }
    public Guid OrderID { get; set; }
    public double Value { get; set; }
}