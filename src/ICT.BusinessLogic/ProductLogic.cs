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

    public void AddStock(int productId, int amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Antal skal være mindst 1.");
        }

        if (!_productAccess.AddStock(productId, amount))
        {
            throw new InvalidOperationException("Varen findes ikke.");
        }
    }
}
