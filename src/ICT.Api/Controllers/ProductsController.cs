using ICT.BusinessLogic;
using ICT.Model;
using Microsoft.AspNetCore.Mvc;

namespace ICT.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductLogic _productLogic;

    public ProductsController(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("ICTOrderDb")!;
        _productLogic = new ProductLogic(connectionString);
    }

    [HttpGet]
    public ActionResult<List<Product>> GetAll()
    {
        return Ok(_productLogic.GetAll());
    }
}
