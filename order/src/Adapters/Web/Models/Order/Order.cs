namespace DevPrime.Web.Models.Order;
public class Order
{
    public string CustomerName { get; set; }
    public string CustomerTaxID { get; set; }
    public IList<DevPrime.Web.Models.Order.Item> Items { get; set; }
    public double Total { get; set; }
    public static Application.Services.Order.Model.Order ToApplication(DevPrime.Web.Models.Order.Order order)
    {
        if (order is null)
            return new Application.Services.Order.Model.Order();
        Application.Services.Order.Model.Order _order = new Application.Services.Order.Model.Order();
        _order.CustomerName = order.CustomerName;
        _order.CustomerTaxID = order.CustomerTaxID;
        _order.Items = DevPrime.Web.Models.Order.Item.ToApplication(order.Items);
        _order.Total = order.Total;
        return _order;
    }
    public static List<Application.Services.Order.Model.Order> ToApplication(IList<DevPrime.Web.Models.Order.Order> orderList)
    {
        List<Application.Services.Order.Model.Order> _orderList = new List<Application.Services.Order.Model.Order>();
        if (orderList != null)
        {
            foreach (var order in orderList)
            {
                Application.Services.Order.Model.Order _order = new Application.Services.Order.Model.Order();
                _order.CustomerName = order.CustomerName;
                _order.CustomerTaxID = order.CustomerTaxID;
                _order.Items = DevPrime.Web.Models.Order.Item.ToApplication(order.Items);
                _order.Total = order.Total;
                _orderList.Add(_order);
            }
        }
        return _orderList;
    }
    public virtual Application.Services.Order.Model.Order ToApplication()
    {
        var model = ToApplication(this);
        return model;
    }
}