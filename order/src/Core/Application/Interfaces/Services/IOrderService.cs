namespace Application.Interfaces.Services;
public interface IOrderService
{
    void Add(Application.Services.Order.Model.Order command);
    void Update(Application.Services.Order.Model.Order command);
    void Delete(Application.Services.Order.Model.Order command);
    Application.Services.Order.Model.Order Get(Application.Services.Order.Model.Order query);
    PagingResult<IList<Application.Services.Order.Model.Order>> GetAll(Application.Services.Order.Model.Order query);
}