namespace DevPrime.State.Repositories.Payment;
public class PaymentRepository : RepositoryBase, IPaymentRepository
{
    public PaymentRepository(IDpState dp) : base(dp)
    {
        ConnectionAlias = "State1";
    }

#region Write

    public void Add(Domain.Aggregates.Payment.Payment payment)
    {
        Dp.Pipeline(Execute: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext);
            var _payment = ToState(payment);
            state.Payment.InsertOne(_payment);
        });
    }

    public void Delete(Guid paymentID)
    {
        Dp.Pipeline(Execute: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext);
            state.Payment.DeleteOne(p => p.PaymentID == paymentID);
        });
    }

    public void Update(Domain.Aggregates.Payment.Payment payment)
    {
        Dp.Pipeline(Execute: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext);
            var _payment = ToState(payment);
            _payment.Id = state.Payment.Find(p => p.PaymentID == payment.ID).FirstOrDefault().Id;
            state.Payment.ReplaceOne(p => p.PaymentID == payment.ID, _payment);
        });
    }

#endregion Write

#region Read

    public Domain.Aggregates.Payment.Payment Get(Guid paymentID)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext);
            var payment = state.Payment.Find(p => p.PaymentID == paymentID).FirstOrDefault();
            var _payment = ToDomain(payment);
            return _payment;
        });
    }

    public List<Domain.Aggregates.Payment.Payment> GetAll(int? limit, int? offset, string ordering, string sort, string filter)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext);
            if (limit is null)
                limit = 1;
            if (offset is null)
                offset = 1;
            List<Model.Payment> payment = null;
            if (sort?.ToLower() == "desc")
            {
                payment = state.Payment.Find(GetFilter(filter)).SortByDescending(GetOrdering(ordering)).Skip((offset - 1) * limit).Limit(limit).ToList();
            }
            else
            {
                payment = state.Payment.Find(GetFilter(filter)).SortBy(GetOrdering(ordering)).Skip((offset - 1) * limit).Limit(limit).ToList();
            }
            var _payment = ToDomain(payment);
            return _payment;
        });
    }
    private Expression<Func<Model.Payment, object>> GetOrdering(string field)
    {
        Expression<Func<Model.Payment, object>> exp = p => p.PaymentID;
        if (!string.IsNullOrWhiteSpace(field))
        {
            if (field.ToLower() == "customername")
                exp = p => p.CustomerName;
            else if (field.ToLower() == "orderid")
                exp = p => p.OrderID;
            else if (field.ToLower() == "value")
                exp = p => p.Value;
            else
                exp = p => p.PaymentID;
        }
        return exp;
    }
    private Expression<Func<Model.Payment, bool>> GetFilter(string filter)
    {
        Expression<Func<Model.Payment, bool>> exp = p => true;
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var slice = filter?.Split("=");
            if (slice.Length > 1)
            {
                var field = slice[0];
                var value = slice[1];
                if (field.ToLower() == "customername")
                    exp = p => p.CustomerName.ToLower() == value.ToLower();
                else if (field.ToLower() == "orderid")
                    exp = p => p.OrderID == new Guid(value);
                else if (field.ToLower() == "value")
                    exp = p => p.Value == Convert.ToDouble(value);
                else
                    exp = p => true;
            }
        }
        return exp;
    }

    public bool Exists(Guid paymentID)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext);
            var payment = state.Payment.Find(x => x.PaymentID == paymentID).Project<Model.Payment>("{ PaymentID: 1 }").FirstOrDefault();
            return (paymentID == payment?.PaymentID);
        });
    }

    public long Total(string filter)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext);
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
        _payment.PaymentID = payment.ID;
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
        Domain.Aggregates.Payment.Payment _payment = new Domain.Aggregates.Payment.Payment(payment.PaymentID, payment.CustomerName, payment.OrderID, payment.Value);
        return _payment;
    }

    public static List<Domain.Aggregates.Payment.Payment> ToDomain(IList<DevPrime.State.Repositories.Payment.Model.Payment> paymentList)
    {
        List<Domain.Aggregates.Payment.Payment> _paymentList = new List<Domain.Aggregates.Payment.Payment>();
        if (paymentList != null)
        {
            foreach (var payment in paymentList)
            {
                Domain.Aggregates.Payment.Payment _payment = new Domain.Aggregates.Payment.Payment(payment.PaymentID, payment.CustomerName, payment.OrderID, payment.Value);
                _paymentList.Add(_payment);
            }
        }
        return _paymentList;
    }

#endregion mappers
}