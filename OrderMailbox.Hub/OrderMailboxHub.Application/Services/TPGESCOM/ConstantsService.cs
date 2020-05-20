using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMailboxHub.Application.Dtos.Catalog;
using OrderMailboxHub.Application.Dtos.Constants;
using OrderMailboxHub.Application.Services.Common;
using OrderMailboxHub.Application.Services.TPGESCOM.Configuration;
using Seedwork.CrossCutting.HttpClientInvoker;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.TPGESCOM
{
    public class ConstantsService : MgmtService<ConstantsDto, TPGESCOMApiConfiguration> , IConstantsService
    {
        private readonly ILogger<ConstantsService> _logger;

        public ConstantsService(IOptions<TPGESCOMApiConfiguration> TPGESCOMApiConfiguration,
            IRestClientBase restClientBase, IPolicyFaultSolver policyFaultSolver, ILogger<ConstantsService> logger)
            : base(TPGESCOMApiConfiguration, restClientBase, policyFaultSolver)
        {
            _logger = logger;
        }

        public override string Resource => _apiConfiguration.EndpointByKey("constants");

        public Task<List<ConstantsDto>> GetAllConstants(string countryId)
        => ExecuteAndCaptureWithPolicyAsync(() =>
        {
            _logger.LogInformation($"URL --> {_apiConfiguration.BaseUrl}{Resource}  --> QUERY --> {nameof(countryId)}={countryId} ");
            return _restClientBase.GetAsync<List<ConstantsDto>>(_apiConfiguration.BaseUrl, $"{Resource}?countryId={countryId}");
        });
    }
}
