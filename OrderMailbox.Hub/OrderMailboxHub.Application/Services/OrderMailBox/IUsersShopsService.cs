
using OrderMailboxHub.Application.Dtos.Orders;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.OrderMailBox
{
    public interface IUsersShopsService
    {
        Task<string> CreateUsersShopsAsync(CreateUsersShopsCommand command);
    }
}
