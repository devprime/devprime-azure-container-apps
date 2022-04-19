namespace Application.Interfaces.Adapters.State;
public interface IPaymentRepository
{
    void Add(Domain.Aggregates.Payment.Payment source);
    void Delete(Guid Id);
    void Update(Domain.Aggregates.Payment.Payment source);
    Domain.Aggregates.Payment.Payment Get(Guid Id);
    List<Domain.Aggregates.Payment.Payment> GetAll(int? limit, int? offset, string ordering, string sort, string filter);
    bool Exists(Guid id);
    long Total(string filter);
}