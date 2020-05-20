using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMailboxHub.Application.Services.Common;
using Seedwork.CrossCutting.HttpClientInvoker;
using OrderMailboxHub.Application.Services.OrderMailBox.Configuration;
using OrderMailboxHub.Application.Dtos.Orders;

namespace OrderMailboxHub.Application.Services.OrderMailBox
{
    public class OrdersService : MgmtService<OrderModel, OrderMailBoxApiConfiguration>, IOrdersService
    {
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(IOptions<OrderMailBoxApiConfiguration> orderMailBoxApiConfiguration,
            IRestClientBase restClientBase, IPolicyFaultSolver policyFaultSolver, ILogger<OrdersService> logger)
            : base(orderMailBoxApiConfiguration, restClientBase, policyFaultSolver)
        {
            _logger = logger;
        }

        public override string Resource => _apiConfiguration.EndpointByKey("orders");

        public Task<List<OrderModel>> GetOrdersByFilter(GetOrdersQuery getOrdersQuery)
        => ExecuteAndCaptureWithPolicyAsync(() =>
        {
            _logger.LogInformation($"URL --> {_apiConfiguration.BaseUrl}/{Resource}  --> BODY --> {nameof(getOrdersQuery)}={getOrdersQuery} ");
            return _restClientBase.PostAsync<List<OrderModel>>(_apiConfiguration.BaseUrl, $"{Resource}", getOrdersQuery);
        });

        public Task<string> GetDetailsOrderByFilter(string orderId)
       => ExecuteAndCaptureWithPolicyAsync(() =>
       {
           _logger.LogInformation($"URL --> {_apiConfiguration.BaseUrl}/{Resource}/{orderId}/xml");
           return _restClientBase.GetAsync<string>(_apiConfiguration.BaseUrl, $"{Resource}/{orderId}/xml");
       });

        public Task<string> ChangeStateOrder(ChangeOrderStateCommand changeOrderStateCommand)
      => ExecuteAndCaptureWithPolicyAsync(() =>
      {
          _logger.LogInformation($"URL --> {_apiConfiguration.BaseUrl}/{Resource}  --> BODY --> {nameof(changeOrderStateCommand)}={changeOrderStateCommand} ");
          return _restClientBase.PutAsync<string>(_apiConfiguration.BaseUrl, $"{Resource}/change-order-state", changeOrderStateCommand);
      });
    }
}
