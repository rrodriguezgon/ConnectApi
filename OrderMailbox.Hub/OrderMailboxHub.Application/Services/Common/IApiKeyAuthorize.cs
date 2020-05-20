using OrderMailboxHub.Application.Dtos.Orders;
using Seedwork.IAM.Services.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.Common
{
    public interface IApiKeyAuthorize
    {
        Task<UserShopModel> GetShopAsync(string shopId);

        string GetUserId();
        string GetCountryId();
    }
}