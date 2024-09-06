namespace MASA.Admin.Service.Controllers;

[ApiController]
[Route("api/v1/[controller]s/[action]")]
public class OrderController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Order> QueryList([FromServices] IEventBus eventBus)
    {
        var orderQueryEvent = new QueryOrderListEvent();
        eventBus.PublishAsync(orderQueryEvent);
        return orderQueryEvent.Orders;
    }

}