namespace Application.Interfaces.Adapters.State;
public interface IOrderRepository
{
    void Add(Domain.Aggregates.Order.Order source);
    void Delete(Guid Id);
    void Update(Domain.Aggregates.Order.Order source);
    Domain.Aggregates.Order.Order Get(Guid Id);
    List<Domain.Aggregates.Order.Order> GetAll(int? limit, int? offset, string ordering, string sort, string filter);
    bool Exists(Guid id);
    long Total(string filter);
}