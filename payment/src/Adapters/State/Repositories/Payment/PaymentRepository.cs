namespace DevPrime.State.Repositories.Payment;
public class PaymentRepository : RepositoryBase, IPaymentRepository
{
    public PaymentRepository(IDpState dp) : base(dp)
    {
        ConnectionAlias = "State1";
    }

#region Write
    public bool Add(Domain.Aggregates.Payment.Payment payment)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var _payment = ToState(payment);
            state.Payment.InsertOne(_payment);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }
    public bool Delete(Guid paymentID)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            state.Payment.DeleteOne(p => p.ID == paymentID);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }
    public bool Update(Domain.Aggregates.Payment.Payment payment)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var _payment = ToState(payment);
            _payment._Id = state.Payment.Find(p => p.ID == payment.ID).FirstOrDefault()._Id;
            state.Payment.ReplaceOne(p => p.ID == payment.ID, _payment);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }

#endregion Write

#region Read
    public Domain.Aggregates.Payment.Payment Get(Guid paymentID)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var payment = state.Payment.Find(p => p.ID == paymentID).FirstOrDefault();
            var _payment = ToDomain(payment);
            return _payment;
        });
    }
    public List<Domain.Aggregates.Payment.Payment> GetAll(int? limit, int? offset, string ordering, string sort, string filter)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            List<Model.Payment> payment = null;
            if (sort?.ToLower() == "desc")
            {
                var result = state.Payment.Find(GetFilter(filter)).SortByDescending(GetOrdering(ordering));
                if (limit != null && offset != null)
                    payment = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    payment = result.ToList();
            }
            else if (sort?.ToLower() == "asc")
            {
                var result = state.Payment.Find(GetFilter(filter)).SortBy(GetOrdering(ordering));
                if (limit != null && offset != null)
                    payment = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    payment = result.ToList();
            }
            else
            {
                var result = state.Payment.Find(GetFilter(filter));
                if (limit != null && offset != null)
                    payment = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    payment = result.ToList();
            }
            var _payment = ToDomain(payment);
            return _payment;
        });
    }
    private Expression<Func<Model.Payment, object>> GetOrdering(string field)
    {
        Expression<Func<Model.Payment, object>> exp = p => p.ID;
        if (!string.IsNullOrWhiteSpace(field))
        {
            if (field.ToLower() == "customername")
                exp = p => p.CustomerName;
            else if (field.ToLower() == "orderid")
                exp = p => p.OrderID;
            else if (field.ToLower() == "value")
                exp = p => p.Value;
            else
                exp = p => p.ID;
        }
        return exp;
    }
    private FilterDefinition<Model.Payment> GetFilter(string filter)
    {
        var builder = Builders<Model.Payment>.Filter;
        FilterDefinition<Model.Payment> exp;
        string CustomerName = string.Empty;
        Guid? OrderID = null;
        Double? Value = null;
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
                        else if (field.ToLower() == "orderid")
                            OrderID = new Guid(value);
                        else if (field.ToLower() == "value")
                            Value = Convert.ToDouble(value);
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
        if (OrderID != null)
        {
            var OrderIDFilter = builder.Eq(x => x.OrderID, OrderID);
            bfilter &= OrderIDFilter;
        }
        if (Value != null)
        {
            var ValueFilter = builder.Eq(x => x.Value, Value);
            bfilter &= ValueFilter;
        }
        exp = bfilter;
        return exp;
    }
    public bool Exists(Guid paymentID)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var payment = state.Payment.Find(x => x.ID == paymentID).Project<Model.Payment>("{ ID: 1 }").FirstOrDefault();
            return (paymentID == payment?.ID);
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
            var total = state.Payment.Find(GetFilter(filter)).CountDocuments();
            return total;
        });
    }

#endregion Read

#region mappers
    public static DevPrime.State.Repositories.Payment.Model.Payment ToState(Domain.Aggregates.Payment.Payment payment)
    {
        if (payment is null)
            return new DevPrime.State.Repositories.Payment.Model.Payment();
        DevPrime.State.Repositories.Payment.Model.Payment _payment = new DevPrime.State.Repositories.Payment.Model.Payment();
        _payment.ID = payment.ID;
        _payment.CustomerName = payment.CustomerName;
        _payment.OrderID = payment.OrderID;
        _payment.Value = payment.Value;
        return _payment;
    }
    public static Domain.Aggregates.Payment.Payment ToDomain(DevPrime.State.Repositories.Payment.Model.Payment payment)
    {
        if (payment is null)
            return new Domain.Aggregates.Payment.Payment()
            {IsNew = true};
        Domain.Aggregates.Payment.Payment _payment = new Domain.Aggregates.Payment.Payment(payment.ID, payment.CustomerName, payment.OrderID, payment.Value);
        return _payment;
    }
    public static List<Domain.Aggregates.Payment.Payment> ToDomain(IList<DevPrime.State.Repositories.Payment.Model.Payment> paymentList)
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

#endregion mappers
}