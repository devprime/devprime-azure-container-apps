namespace Application.Interfaces.Adapters.State;
public interface IPaymentRepository
{
    bool Add(Domain.Aggregates.Payment.Payment source);
    bool Delete(Guid Id);
    bool Update(Domain.Aggregates.Payment.Payment source);
    Domain.Aggregates.Payment.Payment Get(Guid Id);
    List<Domain.Aggregates.Payment.Payment> GetAll(int? limit, int? offset, string ordering, string sort, string filter);
    bool Exists(Guid id);
    long Total(string filter);
}