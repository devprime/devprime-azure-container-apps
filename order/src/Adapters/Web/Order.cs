namespace DevPrime.Web;
public class Order : Routes
{
    public override void Endpoints(WebApplication app)
    {
        //Automatically returns 404 when no result  
        app.MapGet("/v1/order", async (HttpContext http, IOrderService Service, int? limit, int? offset, string ordering, string ascdesc, string filter) => await Dp(http).Pipeline(() => Service.GetAll(new Application.Services.Order.Model.Order(limit, offset, ordering, ascdesc, filter)), 404));
        //Automatically returns 404 when no result 
        app.MapGet("/v1/order/{id}", async (HttpContext http, IOrderService Service, Guid id) => await Dp(http).Pipeline(() => Service.Get(new Application.Services.Order.Model.Order(id)), 404));
        app.MapPost("/v1/order", async (HttpContext http, IOrderService Service, DevPrime.Web.Models.Order.Order command) => await Dp(http).Pipeline(() => Service.Add(command.ToApplication())));
        app.MapPut("/v1/order", async (HttpContext http, IOrderService Service, Application.Services.Order.Model.Order command) => await Dp(http).Pipeline(() => Service.Update(command)));
        app.MapDelete("/v1/order/{id}", async (HttpContext http, IOrderService Service, Guid id) => await Dp(http).Pipeline(() => Service.Delete(new Application.Services.Order.Model.Order(id))));
    }
}