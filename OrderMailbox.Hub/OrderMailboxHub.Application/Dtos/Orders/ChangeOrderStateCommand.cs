
namespace OrderMailboxHub.Application.Dtos.Orders
{
    public class ChangeOrderStateCommand
    {
        public string orderId { get; set; }
        public int orderStateId { get; set; }
        public string comment { get; set; }
    }
}
