namespace Application.Interfaces.Adapters.State;
public interface IOrderRepository
{
    bool Add(Domain.Aggregates.Order.Order source);
    bool Delete(Guid Id);
    bool Update(Domain.Aggregates.Order.Order source);
    Domain.Aggregates.Order.Order Get(Guid Id);
    List<Domain.Aggregates.Order.Order> GetAll(int? limit, int? offset, string ordering, string sort, string filter);
    bool Exists(Guid id);
    long Total(string filter);
}