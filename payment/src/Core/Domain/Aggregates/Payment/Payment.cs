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
            Dp.ProcessEvent(new PaymentCreated());
        });
    }

    public virtual void Update()
    {
        Dp.Pipeline(Execute: () =>
        {
            ValidFields();
            Dp.ProcessEvent(new PaymentUpdated());
        });
    }

    public virtual void Delete()
    {
        Dp.Pipeline(Execute: () =>
        {
            if (ID != Guid.Empty)
                Dp.ProcessEvent(new PaymentDeleted());
        });
    }

    public virtual (List<Payment> Result, long Total) Get(int? limit, int? offset, string ordering, string sort, string filter)
    {
        return Dp.Pipeline(ExecuteResult: () =>
        {
            if (offset == null && limit != null)
            {
                throw new PublicException("Offset is required if you have limit");
            }
            else if (offset != null && limit == null)
            {
                throw new PublicException("Limit is required if you have offset");
            }
            else if (offset != null && limit != null)
            {
                if (offset < 1)
                    throw new PublicException("Offset must be greater than 1");
                if (limit < 1)
                    throw new PublicException("Limit must be greater than 1");
            }
            if (string.IsNullOrWhiteSpace(ordering) && !string.IsNullOrWhiteSpace(sort))
            {
                throw new PublicException("Ordering is required if you have sort");
            }
            else if (!string.IsNullOrWhiteSpace(ordering) && string.IsNullOrWhiteSpace(sort))
            {
                throw new PublicException("Sort is required if you have ordering");
            }
            else if (!string.IsNullOrWhiteSpace(sort) && !string.IsNullOrWhiteSpace(ordering))
            {
                if (sort?.ToLower() != "desc" && sort?.ToLower() != "asc")
                    throw new PublicException("Sort must be 'Asc' or 'Desc'");
                bool orderingIsValid = false;
                if (ordering?.ToLower() == "id")
                    orderingIsValid = true;
                if (ordering?.ToLower() == "customername")
                    orderingIsValid = true;
                if (ordering?.ToLower() == "orderid")
                    orderingIsValid = true;
                if (ordering?.ToLower() == "value")
                    orderingIsValid = true;
                if (!orderingIsValid)
                    throw new PublicException($"Ordering '{ordering}' is invalid try: 'ID=somevalue', 'CustomerName=somevalue', 'OrderID=somevalue', 'Value=somevalue',");
            }
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
    private void ValidFields()
    {
        if (String.IsNullOrWhiteSpace(CustomerName))
            Dp.Notifications.Add("CustomerName is required");
        Dp.Notifications.ValidateAndThrow();
    }
}