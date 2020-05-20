using OrderMailboxHub.Application.Dtos.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.OrderMailBox
{
    public interface IOrdersService
    {
        Task<List<OrderModel>> GetOrdersByFilter(GetOrdersQuery filter);
        Task<string> GetDetailsOrderByFilter(string orderId);
        Task<string> ChangeStateOrder(ChangeOrderStateCommand changeOrderStateCommand);
    }
}
