namespace Application.Services.Payment.Model;
public class Payment
{
    internal int? Limit { get; set; }
    internal int? Offset { get; set; }
    internal string Ordering { get; set; }
    internal string Filter { get; set; }
    internal string Sort { get; set; }
    public Payment(int? limit, int? offset, string ordering, string sort, string filter)
    {
        Limit = limit;
        Offset = offset;
        Ordering = ordering;
        Filter = filter;
        Sort = sort;
    }
    public Guid ID { get; set; }
    public string CustomerName { get; set; }
    public Guid OrderID { get; set; }
    public double Value { get; set; }
    public virtual PagingResult<IList<Payment>> ToPaymentList(IList<Domain.Aggregates.Payment.Payment> paymentList, long? total, int? offSet, int? limit)
    {
        var _paymentList = ToApplication(paymentList);
        return new PagingResult<IList<Payment>>(_paymentList, total, offSet, limit);
    }
    public virtual Payment ToPayment(Domain.Aggregates.Payment.Payment payment)
    {
        var _payment = ToApplication(payment);
        return _payment;
    }
    public virtual Domain.Aggregates.Payment.Payment ToDomain()
    {
        var _payment = ToDomain(this);
        return _payment;
    }
    public virtual Domain.Aggregates.Payment.Payment ToDomain(Guid id)
    {
        var _payment = new Domain.Aggregates.Payment.Payment();
        _payment.ID = id;
        return _payment;
    }
    public Payment()
    {
    }
    public Payment(Guid id)
    {
        ID = id;
    }
    public static Application.Services.Payment.Model.Payment ToApplication(Domain.Aggregates.Payment.Payment payment)
    {
        if (payment is null)
            return new Application.Services.Payment.Model.Payment();
        Application.Services.Payment.Model.Payment _payment = new Application.Services.Payment.Model.Payment();
        _payment.ID = payment.ID;
        _payment.CustomerName = payment.CustomerName;
        _payment.OrderID = payment.OrderID;
        _payment.Value = payment.Value;
        return _payment;
    }
    public static List<Application.Services.Payment.Model.Payment> ToApplication(IList<Domain.Aggregates.Payment.Payment> paymentList)
    {
        List<Application.Services.Payment.Model.Payment> _paymentList = new List<Application.Services.Payment.Model.Payment>();
        if (paymentList != null)
        {
            foreach (var payment in paymentList)
            {
                Application.Services.Payment.Model.Payment _payment = new Application.Services.Payment.Model.Payment();
                _payment.ID = payment.ID;
                _payment.CustomerName = payment.CustomerName;
                _payment.OrderID = payment.OrderID;
                _payment.Value = payment.Value;
                _paymentList.Add(_payment);
            }
        }
        return _paymentList;
    }
    public static Domain.Aggregates.Payment.Payment ToDomain(Application.Services.Payment.Model.Payment payment)
    {
        if (payment is null)
            return new Domain.Aggregates.Payment.Payment();
        Domain.Aggregates.Payment.Payment _payment = new Domain.Aggregates.Payment.Payment(payment.ID, payment.CustomerName, payment.OrderID, payment.Value);
        return _payment;
    }
    public static List<Domain.Aggregates.Payment.Payment> ToDomain(IList<Application.Services.Payment.Model.Payment> paymentList)
    {
        List<Domain.Aggregates.Payment.Payment> _paymentList = new List<Domain.Aggregates.Payment.Payment>();
        if (paymentList != null)
        {
            foreach (var payment in paymentList)
            {
                Domain.Aggregates.Payment.Payment _payment = new Domain.Aggregates.Payment.Payment(payment.ID, payment.CustomerName, payment.OrderID, payment.Value);
                _paymentList.Add(_payment);
            }
        }
        return _paymentList;
    }
}