namespace Application.Services.Payment;
public class PaymentService : ApplicationService<IPaymentState>, IPaymentService
{
    public PaymentService(IPaymentState state, IDp dp) : base(state, dp)
    {
    }
    public void Add(Model.Payment command)
    {
        Dp.Pipeline(Execute: () =>
        {
            var payment = command.ToDomain();
            Dp.Attach(payment);
            payment.Add();
        });
    }
    public void Update(Model.Payment command)
    {
        Dp.Pipeline(Execute: () =>
        {
            var payment = command.ToDomain();
            Dp.Attach(payment);
            payment.Update();
        });
    }
    public void Delete(Model.Payment command)
    {
        Dp.Pipeline(Execute: () =>
        {
            var payment = command.ToDomain();
            Dp.Attach(payment);
            payment.Delete();
        });
    }
    public PagingResult<IList<Model.Payment>> GetAll(Model.Payment query)
    {
        return Dp.Pipeline(ExecuteResult: () =>
        {
            var payment = query.ToDomain();
            Dp.Attach(payment);
            var paymentList = payment.Get(query.Limit, query.Offset, query.Ordering, query.Sort, query.Filter);
            var result = query.ToPaymentList(paymentList.Result, paymentList.Total, query.Offset, query.Limit);
            return result;
        });
    }
    public Model.Payment Get(Model.Payment query)
    {
        return Dp.Pipeline(ExecuteResult: () =>
        {
            var payment = query.ToDomain();
            Dp.Attach(payment);
            payment = payment.GetByID();
            var result = query.ToPayment(payment);
            return result;
        });
    }
}