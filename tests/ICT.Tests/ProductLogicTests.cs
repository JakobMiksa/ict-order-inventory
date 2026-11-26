using ICT.BusinessLogic;

namespace ICT.Tests;

public class ProductLogicTests
{
    [Fact]
    public void AddStock_WithZero_IsRejected()
    {
        ProductLogic logic = new ProductLogic("Server=ikke-brugt");

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => logic.AddStock(1, 0));

        Assert.Equal("Antal skal være mindst 1.", ex.Message);
    }

    [Fact]
    public void AddStock_WithNegativeAmount_IsRejected()
    {
        ProductLogic logic = new ProductLogic("Server=ikke-brugt");

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => logic.AddStock(1, -5));

        Assert.Equal("Antal skal være mindst 1.", ex.Message);
    }
}
