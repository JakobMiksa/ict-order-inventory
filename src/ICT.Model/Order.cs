namespace ICT.Model;

public class Order
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int OrderRoundId { get; set; }
    public List<OrderLine> Lines { get; set; } = new List<OrderLine>();
}
