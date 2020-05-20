using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMailboxHub.Application.Dtos.Catalog;
using OrderMailboxHub.Application.Services.Common;
using OrderMailboxHub.Application.Services.TPGESCOM.Configuration;
using Seedwork.CrossCutting.HttpClientInvoker;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.TPGESCOM
{
    public class CatalogService : MgmtService<Product, TPGESCOMApiConfiguration> , ICatalogService
    {
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(IOptions<TPGESCOMApiConfiguration> orderMailBoxApiConfiguration,
            IRestClientBase restClientBase, IPolicyFaultSolver policyFaultSolver, ILogger<CatalogService> logger)
            : base(orderMailBoxApiConfiguration, restClientBase, policyFaultSolver)
        {
            _logger = logger;
        }

        public override string Resource => _apiConfiguration.EndpointByKey("catalog");

        public Task<List<Product>> GetProductsByFilter(CatalogFilterDto catalogFilterDto)
        => ExecuteAndCaptureWithPolicyAsync(() =>
        {
            _logger.LogInformation($"URL --> {_apiConfiguration.BaseUrl}{Resource}/products  --> BODY -->");
            return _restClientBase.GetAsync<List<Product>>(_apiConfiguration.BaseUrl, $"{Resource}/products?idShop={catalogFilterDto.idShop}&countryId={catalogFilterDto.countryId}");
        });

        public Task<List<PromotionDto>> GetPromotionsByFilter(PromotionFilterDto promotionFilterDto)
        => ExecuteAndCaptureWithPolicyAsync(() =>
        {
            _logger.LogInformation($"URL --> {_apiConfiguration.BaseUrl}/{Resource}/promotions  --> BODY --> {nameof(promotionFilterDto)}={promotionFilterDto} ");
            return _restClientBase.GetAsync<List<PromotionDto>>(_apiConfiguration.BaseUrl, $"{Resource}/promotions?idShop={promotionFilterDto.idShop}&countryId={promotionFilterDto.countryId}");            
        });
    }
}
