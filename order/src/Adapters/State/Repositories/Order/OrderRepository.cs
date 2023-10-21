namespace DevPrime.State.Repositories.Order;
public class OrderRepository : RepositoryBase, IOrderRepository
{
    public OrderRepository(IDpState dp) : base(dp)
    {
        ConnectionAlias = "State1";
    }

#region Write
    public bool Add(Domain.Aggregates.Order.Order order)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var _order = ToState(order);
            state.Order.InsertOne(_order);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }
    public bool Delete(Guid orderID)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            state.Order.DeleteOne(p => p.ID == orderID);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }
    public bool Update(Domain.Aggregates.Order.Order order)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var _order = ToState(order);
            _order._Id = state.Order.Find(p => p.ID == order.ID).FirstOrDefault()._Id;
            state.Order.ReplaceOne(p => p.ID == order.ID, _order);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }

#endregion Write

#region Read
    public Domain.Aggregates.Order.Order Get(Guid orderID)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var order = state.Order.Find(p => p.ID == orderID).FirstOrDefault();
            var _order = ToDomain(order);
            return _order;
        });
    }
    public List<Domain.Aggregates.Order.Order> GetAll(int? limit, int? offset, string ordering, string sort, string filter)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            List<Model.Order> order = null;
            if (sort?.ToLower() == "desc")
            {
                var result = state.Order.Find(GetFilter(filter)).SortByDescending(GetOrdering(ordering));
                if (limit != null && offset != null)
                    order = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    order = result.ToList();
            }
            else if (sort?.ToLower() == "asc")
            {
                var result = state.Order.Find(GetFilter(filter)).SortBy(GetOrdering(ordering));
                if (limit != null && offset != null)
                    order = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    order = result.ToList();
            }
            else
            {
                var result = state.Order.Find(GetFilter(filter));
                if (limit != null && offset != null)
                    order = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    order = result.ToList();
            }
            var _order = ToDomain(order);
            return _order;
        });
    }
    private Expression<Func<Model.Order, object>> GetOrdering(string field)
    {
        Expression<Func<Model.Order, object>> exp = p => p.ID;
        if (!string.IsNullOrWhiteSpace(field))
        {
            if (field.ToLower() == "customername")
                exp = p => p.CustomerName;
            else if (field.ToLower() == "customertaxid")
                exp = p => p.CustomerTaxID;
            else if (field.ToLower() == "total")
                exp = p => p.Total;
            else
                exp = p => p.ID;
        }
        return exp;
    }
    private FilterDefinition<Model.Order> GetFilter(string filter)
    {
        var builder = Builders<Model.Order>.Filter;
        FilterDefinition<Model.Order> exp;
        string CustomerName = string.Empty;
        string CustomerTaxID = string.Empty;
        Double? Total = null;
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var conditions = filter.Split(",");
            if (conditions.Count() >= 1)
            {
                foreach (var condition in conditions)
                {
                    var slice = condition?.Split("=");
                    if (slice.Length > 1)
                    {
                        var field = slice[0];
                        var value = slice[1];
                        if (field.ToLower() == "customername")
                            CustomerName = value;
                        else if (field.ToLower() == "customertaxid")
                            CustomerTaxID = value;
                        else if (field.ToLower() == "total")
                            Total = Convert.ToDouble(value);
                    }
                }
            }
        }
        var bfilter = builder.Empty;
        if (!string.IsNullOrWhiteSpace(CustomerName))
        {
            var CustomerNameFilter = builder.Eq(x => x.CustomerName, CustomerName);
            bfilter &= CustomerNameFilter;
        }
        if (!string.IsNullOrWhiteSpace(CustomerTaxID))
        {
            var CustomerTaxIDFilter = builder.Eq(x => x.CustomerTaxID, CustomerTaxID);
            bfilter &= CustomerTaxIDFilter;
        }
        if (Total != null)
        {
            var TotalFilter = builder.Eq(x => x.Total, Total);
            bfilter &= TotalFilter;
        }
        exp = bfilter;
        return exp;
    }
    public bool Exists(Guid orderID)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var order = state.Order.Find(x => x.ID == orderID).Project<Model.Order>("{ ID: 1 }").FirstOrDefault();
            return (orderID == order?.ID);
        });
        if (result is null)
            return false;
        return result;
    }
    public long Total(string filter)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var total = state.Order.Find(GetFilter(filter)).CountDocuments();
            return total;
        });
    }

#endregion Read

#region mappers
    public static DevPrime.State.Repositories.Order.Model.Order ToState(Domain.Aggregates.Order.Order order)
    {
        if (order is null)
            return new DevPrime.State.Repositories.Order.Model.Order();
        DevPrime.State.Repositories.Order.Model.Order _order = new DevPrime.State.Repositories.Order.Model.Order();
        _order.ID = order.ID;
        _order.CustomerName = order.CustomerName;
        _order.CustomerTaxID = order.CustomerTaxID;
        _order.Items = ToState(order.Items);
        _order.Total = order.Total;
        return _order;
    }
    public static DevPrime.State.Repositories.Order.Model.Item ToState(Domain.Aggregates.Order.Item item)
    {
        if (item is null)
            return new DevPrime.State.Repositories.Order.Model.Item();
        DevPrime.State.Repositories.Order.Model.Item _item = new DevPrime.State.Repositories.Order.Model.Item();
        _item.ID = item.ID;
        _item.Description = item.Description;
        _item.Amount = item.Amount;
        _item.SKU = item.SKU;
        _item.Price = item.Price;
        return _item;
    }
    public static List<DevPrime.State.Repositories.Order.Model.Item> ToState(IList<Domain.Aggregates.Order.Item> itemList)
    {
        List<DevPrime.State.Repositories.Order.Model.Item> _itemList = new List<DevPrime.State.Repositories.Order.Model.Item>();
        if (itemList != null)
        {
            foreach (var item in itemList)
            {
                DevPrime.State.Repositories.Order.Model.Item _item = new DevPrime.State.Repositories.Order.Model.Item();
                _item.ID = item.ID;
                _item.Description = item.Description;
                _item.Amount = item.Amount;
                _item.SKU = item.SKU;
                _item.Price = item.Price;
                _itemList.Add(_item);
            }
        }
        return _itemList;
    }
    public static Domain.Aggregates.Order.Order ToDomain(DevPrime.State.Repositories.Order.Model.Order order)
    {
        if (order is null)
            return new Domain.Aggregates.Order.Order()
            {IsNew = true};
        Domain.Aggregates.Order.Order _order = new Domain.Aggregates.Order.Order(order.ID, order.CustomerName, order.CustomerTaxID, ToDomain(order.Items), order.Total);
        return _order;
    }
    public static Domain.Aggregates.Order.Item ToDomain(DevPrime.State.Repositories.Order.Model.Item item)
    {
        if (item is null)
            return new Domain.Aggregates.Order.Item()
            {IsNew = true};
        Domain.Aggregates.Order.Item _item = new Domain.Aggregates.Order.Item(item.ID, item.Description, item.Amount, item.SKU, item.Price);
        return _item;
    }
    public static List<Domain.Aggregates.Order.Item> ToDomain(IList<DevPrime.State.Repositories.Order.Model.Item> itemList)
    {
        List<Domain.Aggregates.Order.Item> _itemList = new List<Domain.Aggregates.Order.Item>();
        if (itemList != null)
        {
            foreach (var item in itemList)
            {
                Domain.Aggregates.Order.Item _item = new Domain.Aggregates.Order.Item(item.ID, item.Description, item.Amount, item.SKU, item.Price);
                _itemList.Add(_item);
            }
        }
        return _itemList;
    }
    public static List<Domain.Aggregates.Order.Order> ToDomain(IList<DevPrime.State.Repositories.Order.Model.Order> orderList)
    {
        List<Domain.Aggregates.Order.Order> _orderList = new List<Domain.Aggregates.Order.Order>();
        if (orderList != null)
        {
            foreach (var order in orderList)
            {
                Domain.Aggregates.Order.Order _order = new Domain.Aggregates.Order.Order(order.ID, order.CustomerName, order.CustomerTaxID, ToDomain(order.Items), order.Total);
                _orderList.Add(_order);
            }
        }
        return _orderList;
    }

#endregion mappers
}