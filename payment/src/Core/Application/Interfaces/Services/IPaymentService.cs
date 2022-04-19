namespace Application.Interfaces.Services;
public interface IPaymentService
{
    void Add(Application.Services.Payment.Model.Payment command);
    void Update(Application.Services.Payment.Model.Payment command);
    void Delete(Application.Services.Payment.Model.Payment command);
    Application.Services.Payment.Model.Payment Get(Application.Services.Payment.Model.Payment query);
    PagingResult<IList<Application.Services.Payment.Model.Payment>> GetAll(Application.Services.Payment.Model.Payment query);
}