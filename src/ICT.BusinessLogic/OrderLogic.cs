using ICT.DataAccess;
using ICT.Model;

namespace ICT.BusinessLogic;

public class OrderLogic
{
    private readonly OrderAccess _orderAccess;
    private readonly OrderRoundAccess _orderRoundAccess;

    public OrderLogic(string connectionString)
    {
        _orderAccess = new OrderAccess(connectionString);
        _orderRoundAccess = new OrderRoundAccess(connectionString);
    }

    public int PlaceOrder(Order order)
    {
        OrderRound? openRound = _orderRoundAccess.GetOpen();
        if (openRound == null)
        {
            throw new InvalidOperationException("Runden er lukket, så du kan ikke bestille lige nu.");
        }

        order.Lines = order.Lines.Where(line => line.Quantity > 0).ToList();
        if (order.Lines.Count == 0)
        {
            throw new InvalidOperationException("Vælg mindst én vare.");
        }

        order.OrderRoundId = openRound.Id;
        return _orderAccess.Create(order);
    }
}
