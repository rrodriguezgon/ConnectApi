using Seedwork.IAM.Services.Models;

namespace OrderMailboxHub.Host.Authorization
{
    public interface IApiKeyAuthorize
    {
        Shop GetShop(string shopId);
    }
}