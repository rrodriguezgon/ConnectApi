using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMailboxHub.Application.Dtos.Orders;
using OrderMailboxHub.Application.Services.Common;
using OrderMailboxHub.Application.Services.OrderMailBox.Configuration;
using Seedwork.CrossCutting.HttpClientInvoker;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderMailboxHub.Application.Services.OrderMailBox
{
    public class UsersShopsService : MgmtService<OrderModel, OrderMailBoxApiConfiguration>, IUsersShopsService
    {
        private readonly ILogger<UsersShopsService> _logger;

        public UsersShopsService(IOptions<OrderMailBoxApiConfiguration> orderMailBoxApiConfiguration,
            IRestClientBase restClientBase, IPolicyFaultSolver policyFaultSolver, ILogger<UsersShopsService> logger)
            : base(orderMailBoxApiConfiguration, restClientBase, policyFaultSolver)
        {
            _logger = logger;
        }

        public override string Resource => _apiConfiguration.EndpointByKey("usersshops");

        public Task<string> CreateUsersShopsAsync(CreateUsersShopsCommand createUsersShopsCommand)
     => ExecuteAndCaptureWithPolicyAsync(() =>
     {
         _logger.LogInformation($"URL --> {_apiConfiguration.BaseUrl}/{Resource}  --> BODY --> {nameof(createUsersShopsCommand)}={createUsersShopsCommand} ");
         return _restClientBase.PostAsync<string>(_apiConfiguration.BaseUrl, $"{Resource}", createUsersShopsCommand);
     });
    }
}
