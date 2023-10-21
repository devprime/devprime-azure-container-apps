namespace DevPrime.Web.Models.Payment;
public class Payment
{
    public string CustomerName { get; set; }
    public Guid OrderID { get; set; }
    public double Value { get; set; }
    public static Application.Services.Payment.Model.Payment ToApplication(DevPrime.Web.Models.Payment.Payment payment)
    {
        if (payment is null)
            return new Application.Services.Payment.Model.Payment();
        Application.Services.Payment.Model.Payment _payment = new Application.Services.Payment.Model.Payment();
        _payment.CustomerName = payment.CustomerName;
        _payment.OrderID = payment.OrderID;
        _payment.Value = payment.Value;
        return _payment;
    }
    public static List<Application.Services.Payment.Model.Payment> ToApplication(IList<DevPrime.Web.Models.Payment.Payment> paymentList)
    {
        List<Application.Services.Payment.Model.Payment> _paymentList = new List<Application.Services.Payment.Model.Payment>();
        if (paymentList != null)
        {
            foreach (var payment in paymentList)
            {
                Application.Services.Payment.Model.Payment _payment = new Application.Services.Payment.Model.Payment();
                _payment.CustomerName = payment.CustomerName;
                _payment.OrderID = payment.OrderID;
                _payment.Value = payment.Value;
                _paymentList.Add(_payment);
            }
        }
        return _paymentList;
    }
    public virtual Application.Services.Payment.Model.Payment ToApplication()
    {
        var model = ToApplication(this);
        return model;
    }
}