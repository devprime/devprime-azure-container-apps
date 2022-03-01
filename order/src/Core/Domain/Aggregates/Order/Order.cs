namespace Domain.Aggregates.Order;
public class Order : AggRoot
{
    public string CustomerName { get; private set; }
    public string CustomerTaxID { get; private set; }
    public IList<Item> Itens { get; private set; }
    public double Total { get; private set; }
    public void AddItem(Item item)
    {
        if (item != null && Itens != null)
        {
            var myItens = Itens.Where(p => p.SKU == item.SKU).FirstOrDefault();
            if (myItens != null)
                myItens.Sum(item.Amount);
            else
                Itens.Add(item);
        }
    }

    public Order(Guid id, string customerName, string customerTaxID, IEnumerable<Domain.Aggregates.Order.Item> itens, double total)
    {
        ID = id;
        CustomerName = customerName;
        CustomerTaxID = customerTaxID;
        Itens = itens?.ToList();
        Total = total;
    }

    public Order()
    {
    }

    public virtual void Add()
    {
        Dp.Pipeline(Execute: () =>
        {
            Dp.Attach(Itens);
            ValidFields();
            ID = Guid.NewGuid();
            IsNew = true;
            Dp.ProcessEvent(new OrderCreated());
        });
    }

    public virtual void Update()
    {
        Dp.Pipeline(Execute: () =>
        {
            Dp.Attach(Itens);
            ValidFields();
            Dp.ProcessEvent(new OrderUpdated());
        });
    }

    public virtual void Delete()
    {
        Dp.Pipeline(Execute: () =>
        {
            if (ID != Guid.Empty)
                Dp.ProcessEvent(new OrderDeleted());
        });
    }

    public virtual (List<Order> Result, long Total) Get(int? limit, int? offset, string ordering, string sort, string filter)
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
                if (ordering?.ToLower() == "customertaxid")
                    orderingIsValid = true;
                if (ordering?.ToLower() == "total")
                    orderingIsValid = true;
                if (!orderingIsValid)
                    throw new PublicException($"Ordering '{ordering}' is invalid try: 'ID=somevalue', 'CustomerName=somevalue', 'CustomerTaxID=somevalue', 'Total=somevalue',");
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
                    if (filter.ToLower().StartsWith("customertaxid="))
                        filterIsValid = true;
                    if (filter.ToLower().StartsWith("total="))
                        filterIsValid = true;
                }
                if (!filterIsValid)
                    throw new PublicException($"Invalid filter '{filter}' is invalid try: 'ID', 'CustomerName', 'CustomerTaxID', 'Total',");
            }
            var source = Dp.ProcessEvent(new OrderGet()
            {Limit = limit, Offset = offset, Ordering = ordering, Sort = sort, Filter = filter});
            return source;
        });
    }
    private void ValidFields()
    {
        if (String.IsNullOrWhiteSpace(CustomerName))
            Dp.Notifications.Add("CustomerName is required");
        if (String.IsNullOrWhiteSpace(CustomerTaxID))
            Dp.Notifications.Add("CustomerTaxID is required");
        Dp.Notifications.ValidateAndThrow();
    }
}