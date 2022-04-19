namespace DevPrime.State.Repositories.Order;
public class OrderRepository : RepositoryBase, IOrderRepository
{
    public OrderRepository(IDpState dp) : base(dp)
    {
        ConnectionAlias = "State1";
    }

#region Write

    public void Add(Domain.Aggregates.Order.Order order)
    {
        Dp.Pipeline(Execute: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var _order = ToState(order);
            state.Order.InsertOne(_order);
        });
    }

    public void Delete(Guid orderID)
    {
        Dp.Pipeline(Execute: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            state.Order.DeleteOne(p => p.OrderID == orderID);
        });
    }

    public void Update(Domain.Aggregates.Order.Order order)
    {
        Dp.Pipeline(Execute: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var _order = ToState(order);
            _order.Id = state.Order.Find(p => p.OrderID == order.ID).FirstOrDefault().Id;
            state.Order.ReplaceOne(p => p.OrderID == order.ID, _order);
        });
    }

#endregion Write

#region Read

    public Domain.Aggregates.Order.Order Get(Guid orderID)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var order = state.Order.Find(p => p.OrderID == orderID).FirstOrDefault();
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
        Expression<Func<Model.Order, object>> exp = p => p.OrderID;
        if (!string.IsNullOrWhiteSpace(field))
        {
            if (field.ToLower() == "customername")
                exp = p => p.CustomerName;
            else if (field.ToLower() == "customertaxid")
                exp = p => p.CustomerTaxID;
            else if (field.ToLower() == "total")
                exp = p => p.Total;
            else
                exp = p => p.OrderID;
        }
        return exp;
    }
    private Expression<Func<Model.Order, bool>> GetFilter(string filter)
    {
        Expression<Func<Model.Order, bool>> exp = p => true;
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var slice = filter?.Split("=");
            if (slice.Length > 1)
            {
                var field = slice[0];
                var value = slice[1];
                if (string.IsNullOrWhiteSpace(value))
                {
                    exp = p => true;
                }
                else
                {
                    if (field.ToLower() == "customername")
                        exp = p => p.CustomerName.ToLower() == value.ToLower();
                    else if (field.ToLower() == "customertaxid")
                        exp = p => p.CustomerTaxID.ToLower() == value.ToLower();
                    else if (field.ToLower() == "total")
                        exp = p => p.Total == Convert.ToDouble(value);
                    else
                        exp = p => true;
                }
            }
        }
        return exp;
    }

    public bool Exists(Guid orderID)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var order = state.Order.Find(x => x.OrderID == orderID).Project<Model.Order>("{ OrderID: 1 }").FirstOrDefault();
            return (orderID == order?.OrderID);
        });
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
        _order.OrderID = order.ID;
        _order.CustomerName = order.CustomerName;
        _order.CustomerTaxID = order.CustomerTaxID;
        _order.Itens = ToState(order.Itens);
        _order.Total = order.Total;
        return _order;
    }

    public static DevPrime.State.Repositories.Order.Model.Item ToState(Domain.Aggregates.Order.Item item)
    {
        if (item is null)
            return new DevPrime.State.Repositories.Order.Model.Item();
        DevPrime.State.Repositories.Order.Model.Item _item = new DevPrime.State.Repositories.Order.Model.Item();
        _item.ItemID = item.ID;
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
                _item.ItemID = item.ID;
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
        Domain.Aggregates.Order.Order _order = new Domain.Aggregates.Order.Order(order.OrderID, order.CustomerName, order.CustomerTaxID, ToDomain(order.Itens), order.Total);
        return _order;
    }

    public static Domain.Aggregates.Order.Item ToDomain(DevPrime.State.Repositories.Order.Model.Item item)
    {
        if (item is null)
            return new Domain.Aggregates.Order.Item()
            {IsNew = true};
        Domain.Aggregates.Order.Item _item = new Domain.Aggregates.Order.Item(item.ItemID, item.Description, item.Amount, item.SKU, item.Price);
        return _item;
    }

    public static List<Domain.Aggregates.Order.Item> ToDomain(IList<DevPrime.State.Repositories.Order.Model.Item> itemList)
    {
        List<Domain.Aggregates.Order.Item> _itemList = new List<Domain.Aggregates.Order.Item>();
        if (itemList != null)
        {
            foreach (var item in itemList)
            {
                Domain.Aggregates.Order.Item _item = new Domain.Aggregates.Order.Item(item.ItemID, item.Description, item.Amount, item.SKU, item.Price);
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
                Domain.Aggregates.Order.Order _order = new Domain.Aggregates.Order.Order(order.OrderID, order.CustomerName, order.CustomerTaxID, ToDomain(order.Itens), order.Total);
                _orderList.Add(_order);
            }
        }
        return _orderList;
    }

#endregion mappers
}