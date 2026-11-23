using ICT.BusinessLogic;
using ICT.Model;

namespace ICT.Tests;

public class OrderLogicTests
{
    [Fact]
    public void PlaceOrder_WithoutLines_IsRejected()
    {
        OrderLogic logic = new OrderLogic("Server=ikke-brugt");
        Order order = new Order();
        order.MemberId = 1;

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => logic.PlaceOrder(order));

        Assert.Equal("Vælg mindst én vare.", ex.Message);
    }

    [Fact]
    public void PlaceOrder_WithOnlyZeroQuantities_IsRejected()
    {
        OrderLogic logic = new OrderLogic("Server=ikke-brugt");
        Order order = new Order();
        order.MemberId = 1;
        order.Lines.Add(new OrderLine { ProductId = 1, Quantity = 0 });
        order.Lines.Add(new OrderLine { ProductId = 2, Quantity = 0 });

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => logic.PlaceOrder(order));

        Assert.Equal("Vælg mindst én vare.", ex.Message);
    }
}
