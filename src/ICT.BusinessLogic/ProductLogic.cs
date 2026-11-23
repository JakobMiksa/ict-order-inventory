using ICT.DataAccess;
using ICT.Model;

namespace ICT.BusinessLogic;

public class ProductLogic
{
    private readonly ProductAccess _productAccess;

    public ProductLogic(string connectionString)
    {
        _productAccess = new ProductAccess(connectionString);
    }

    public List<Product> GetAll()
    {
        return _productAccess.GetAll();
    }
}
