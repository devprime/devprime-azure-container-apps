namespace DevPrime.Web;
public class Payment : Routes
{
    public override void Endpoints(WebApplication app)
    {
        //Automatically returns 404 when no result  
        app.MapGet("/v1/payment", async (HttpContext http, IPaymentService Service, int? limit, int? offset, string ordering, string ascdesc, string filter) => await Dp(http).Pipeline(() => Service.GetAll(new Application.Services.Payment.Model.Payment(limit, offset, ordering, ascdesc, filter)), 404));
        //Automatically returns 404 when no result 
        app.MapGet("/v1/payment/{id}", async (HttpContext http, IPaymentService Service, Guid id) => await Dp(http).Pipeline(() => Service.Get(new Application.Services.Payment.Model.Payment(id)), 404));
        app.MapPost("/v1/payment", async (HttpContext http, IPaymentService Service, DevPrime.Web.Models.Payment.Payment command) => await Dp(http).Pipeline(() => Service.Add(command.ToApplication())));
        app.MapPut("/v1/payment", async (HttpContext http, IPaymentService Service, Application.Services.Payment.Model.Payment command) => await Dp(http).Pipeline(() => Service.Update(command)));
        app.MapDelete("/v1/payment/{id}", async (HttpContext http, IPaymentService Service, Guid id) => await Dp(http).Pipeline(() => Service.Delete(new Application.Services.Payment.Model.Payment(id))));
    }
}