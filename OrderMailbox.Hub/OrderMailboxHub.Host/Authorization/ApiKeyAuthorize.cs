using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seedwork.IAM.Services;
using Seedwork.IAM.Services.Models;

namespace OrderMailboxHub.Host.Authorization
{
    public class ApiKeyAuthorize : IApiKeyAuthorize
    {
        private readonly IIAMServiceClient iamClient;

        public ApiKeyAuthorize(IIAMServiceClient iamClient)
        {
            this.iamClient = iamClient;
        }

        /// <summary>
        /// Obtiene la información de la tienda solo si tiene permismos.
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public Shop GetShop(string shopId)
        {
            return null;
        }
    }
}
