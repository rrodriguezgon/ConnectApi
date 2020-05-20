using Microsoft.AspNetCore.Http;
using OrderMailboxHub.Application.Dtos.Orders;
using OrderMailboxHub.Application.Services.OrderMailBox;
using Seedwork.IAM.Services;
using Seedwork.IAM.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.Common
{
    public class ApiKeyAuthorize : IApiKeyAuthorize
    {
        private readonly IIAMServiceClient iamClient;
        private readonly IHttpContextAccessor httpContext;
        private readonly IUsersShopsService usersShopsService;

        public ApiKeyAuthorize(IIAMServiceClient iamClient, IUsersShopsService usersShopsService, IHttpContextAccessor httpContext)
        {
            this.iamClient = iamClient;
            this.usersShopsService = usersShopsService;
            this.httpContext = httpContext;

        }

        public string GetUserId()
        {
            return GetClaim("http://schemas.microsoft.com/identity/claims/objectidentifier");            
        }

        public string GetCountryId()
        {
           return GetClaim("CountryId");
        }

        public string GetClaim(string Type)
        {
            return httpContext.HttpContext.User.Claims.FirstOrDefault(c =>
                c.Type == Type)?.Value;
        }

        /// <summary>
        /// Obtiene la información de la tienda solo si tiene permismos.
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public async Task<UserShopModel> GetShopAsync(string shopId)
        {
            var myShops = await iamClient.GetMyShops();

            var command = new CreateUsersShopsCommand(GetUserId(), GetUsersShops(GetUserId(), myShops));

            await usersShopsService.CreateUsersShopsAsync(command);

            return command.UsersShops.Where(x => x.ShopId == shopId)
                .FirstOrDefault();
        }

        private IEnumerable<UserShopModel> GetUsersShops(string IdUser, IEnumerable<ShopsData> shopsListData)
        {
            var usuariosTiendas = shopsListData
                .SelectMany(sld => sld.Country, (sld, country) => new { CountryId = country.Code, bussinessArea = country.BussinessArea })
                .SelectMany(c => c.bussinessArea, (sld, bussinnessArea) => new { sld.CountryId, BussinessAreaId = bussinnessArea.Code, operationArea = bussinnessArea.OperationAreas })
                .SelectMany(ba => ba.operationArea, (sld, operationArea) => new { sld.CountryId, sld.BussinessAreaId, operationArea.Supervisors })
                .SelectMany(super => super.Supervisors, (sld, supervisor) => new { sld.CountryId, sld.BussinessAreaId, supervisor.Shops })
                .SelectMany(shop => shop.Shops, (sld, shop) => new { sld.CountryId, sld.BussinessAreaId, ShopId = shop.Code })
                .Select(x => new UserShopModel()
                {
                    CountryId = x.CountryId,
                    BussinessAreaId = x.BussinessAreaId,
                    ShopId = x.ShopId.PadLeft(5, '0')
                });

            return usuariosTiendas;
        }
    }
}
