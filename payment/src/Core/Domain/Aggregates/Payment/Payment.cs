namespace Domain.Aggregates.Payment;
public class Payment : AggRoot
{
    public string CustomerName { get; private set; }
    public Guid OrderID { get; private set; }
    public double Value { get; private set; }
    public Payment(Guid id, string customerName, Guid orderID, double value)
    {
        ID = id;
        CustomerName = customerName;
        OrderID = orderID;
        Value = value;
    }
    public Payment()
    {
    }
    public virtual void Add()
    {
        Dp.Pipeline(Execute: () =>
        {
            ValidFields();
            ID = Guid.NewGuid();
            IsNew = true;
            var success = Dp.ProcessEvent<bool>(new CreatePayment());
            if (success)
            {
                Dp.ProcessEvent(new PaymentCreated());
            }
        });
    }
    public virtual void Update()
    {
        Dp.Pipeline(Execute: () =>
        {
            if (ID.Equals(Guid.Empty))
                Dp.Notifications.Add("ID is required");
            ValidFields();
            var success = Dp.ProcessEvent<bool>(new UpdatePayment());
            if (success)
            {
                Dp.ProcessEvent(new PaymentUpdated());
            }
        });
    }
    public virtual void Delete()
    {
        Dp.Pipeline(Execute: () =>
        {
            if (ID != Guid.Empty)
            {
                var success = Dp.ProcessEvent<bool>(new DeletePayment());
                if (success)
                {
                    Dp.ProcessEvent(new PaymentDeleted());
                }
            }
        });
    }
    public virtual (List<Payment> Result, long Total) Get(int? limit, int? offset, string ordering, string sort, string filter)
    {
        return Dp.Pipeline(ExecuteResult: () =>
        {
            ValidateOrdering(limit, offset, ordering, sort);
            if (!string.IsNullOrWhiteSpace(filter))
            {
                bool filterIsValid = false;
                if (filter.Contains("="))
                {
                    if (filter.ToLower().StartsWith("id="))
                        filterIsValid = true;
                    if (filter.ToLower().StartsWith("customername="))
                        filterIsValid = true;
                    if (filter.ToLower().StartsWith("orderid="))
                        filterIsValid = true;
                    if (filter.ToLower().StartsWith("value="))
                        filterIsValid = true;
                }
                if (!filterIsValid)
                    throw new PublicException($"Invalid filter '{filter}' is invalid try: 'ID', 'CustomerName', 'OrderID', 'Value',");
            }
            var source = Dp.ProcessEvent(new PaymentGet()
            {Limit = limit, Offset = offset, Ordering = ordering, Sort = sort, Filter = filter});
            return source;
        });
    }
    public virtual Payment GetByID()
    {
        var result = Dp.Pipeline(ExecuteResult: () =>
        {
            return Dp.ProcessEvent<Payment>(new PaymentGetByID());
        });
        return result;
    }
    private void ValidFields()
    {
        if (String.IsNullOrWhiteSpace(CustomerName))
            Dp.Notifications.Add("CustomerName is required");
        Dp.Notifications.ValidateAndThrow();
    }
}