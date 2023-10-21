namespace Application.Services.Order.Model;
public class Order
{
    internal int? Limit { get; set; }
    internal int? Offset { get; set; }
    internal string Ordering { get; set; }
    internal string Filter { get; set; }
    internal string Sort { get; set; }
    public Order(int? limit, int? offset, string ordering, string sort, string filter)
    {
        Limit = limit;
        Offset = offset;
        Ordering = ordering;
        Filter = filter;
        Sort = sort;
    }
    public Guid ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerTaxID { get; set; }
    public IList<Application.Services.Order.Model.Item> Items { get; set; }
    public double Total { get; set; }
    public virtual PagingResult<IList<Order>> ToOrderList(IList<Domain.Aggregates.Order.Order> orderList, long? total, int? offSet, int? limit)
    {
        var _orderList = ToApplication(orderList);
        return new PagingResult<IList<Order>>(_orderList, total, offSet, limit);
    }
    public virtual Order ToOrder(Domain.Aggregates.Order.Order order)
    {
        var _order = ToApplication(order);
        return _order;
    }
    public virtual Domain.Aggregates.Order.Order ToDomain()
    {
        var _order = ToDomain(this);
        return _order;
    }
    public virtual Domain.Aggregates.Order.Order ToDomain(Guid id)
    {
        var _order = new Domain.Aggregates.Order.Order();
        _order.ID = id;
        return _order;
    }
    public Order()
    {
    }
    public Order(Guid id)
    {
        ID = id;
    }
    public static Application.Services.Order.Model.Order ToApplication(Domain.Aggregates.Order.Order order)
    {
        if (order is null)
            return new Application.Services.Order.Model.Order();
        Application.Services.Order.Model.Order _order = new Application.Services.Order.Model.Order();
        _order.ID = order.ID;
        _order.CustomerName = order.CustomerName;
        _order.CustomerTaxID = order.CustomerTaxID;
        _order.Items = Application.Services.Order.Model.Item.ToApplication(order.Items);
        _order.Total = order.Total;
        return _order;
    }
    public static List<Application.Services.Order.Model.Order> ToApplication(IList<Domain.Aggregates.Order.Order> orderList)
    {
        List<Application.Services.Order.Model.Order> _orderList = new List<Application.Services.Order.Model.Order>();
        if (orderList != null)
        {
            foreach (var order in orderList)
            {
                Application.Services.Order.Model.Order _order = new Application.Services.Order.Model.Order();
                _order.ID = order.ID;
                _order.CustomerName = order.CustomerName;
                _order.CustomerTaxID = order.CustomerTaxID;
                _order.Items = Application.Services.Order.Model.Item.ToApplication(order.Items);
                _order.Total = order.Total;
                _orderList.Add(_order);
            }
        }
        return _orderList;
    }
    public static Domain.Aggregates.Order.Order ToDomain(Application.Services.Order.Model.Order order)
    {
        if (order is null)
            return new Domain.Aggregates.Order.Order();
        Domain.Aggregates.Order.Order _order = new Domain.Aggregates.Order.Order(order.ID, order.CustomerName, order.CustomerTaxID, Application.Services.Order.Model.Item.ToDomain(order.Items), order.Total);
        return _order;
    }
    public static List<Domain.Aggregates.Order.Order> ToDomain(IList<Application.Services.Order.Model.Order> orderList)
    {
        List<Domain.Aggregates.Order.Order> _orderList = new List<Domain.Aggregates.Order.Order>();
        if (orderList != null)
        {
            foreach (var order in orderList)
            {
                Domain.Aggregates.Order.Order _order = new Domain.Aggregates.Order.Order(order.ID, order.CustomerName, order.CustomerTaxID, Application.Services.Order.Model.Item.ToDomain(order.Items), order.Total);
                _orderList.Add(_order);
            }
        }
        return _orderList;
    }
}