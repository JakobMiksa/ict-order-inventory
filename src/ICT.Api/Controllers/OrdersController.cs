using ICT.BusinessLogic;
using ICT.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICT.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderLogic _orderLogic;

    public OrdersController(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("ICTOrderDb")!;
        _orderLogic = new OrderLogic(connectionString);
    }

    [HttpPost]
    public ActionResult<int> Create(Order order)
    {
        try
        {
            int orderId = _orderLogic.PlaceOrder(order);
            return Ok(orderId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
