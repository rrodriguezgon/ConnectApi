using OrderMailboxHub.Application.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;
using Seedwork.CrossCutting.MVC.Dtos;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using OrderMailboxHub.Application.Services.OrderMailBox;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using OrderMailboxHub.Application.Services.Common;
using System.Security.Claims;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;
using Seedwork.IAM.Services.Models;

namespace OrderMailboxHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "apikey")]
    public class OrdersController : ControllerBase
    {
        private readonly IApiKeyAuthorize _apiKeyAuthorize;
        private readonly IOrdersService _ordersService;
        private readonly ILogger<OrdersController> _log;

        public OrdersController(ILogger<OrdersController> log, IOrdersService ordersService, IApiKeyAuthorize apiKeyAuthorize)
        {
            _apiKeyAuthorize = apiKeyAuthorize;
            _ordersService = ordersService;
            _log = log;
        }

        [ProducesResponseType(typeof(OrderModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPost()]
        public async Task<IActionResult> GetOrders([FromBody]OrderFilterDto orderFilterDto)
        {
            var shop = await _apiKeyAuthorize.GetShopAsync(orderFilterDto.ShopId);
            if (shop == null)
            {
                return Unauthorized();
            }

            string idUser = _apiKeyAuthorize.GetUserId();

            GetOrdersQuery getOrdersQuery = new GetOrdersQuery(idUser, orderFilterDto);
            var result = await _ordersService.GetOrdersByFilter(getOrdersQuery);

            return GetResponseResult(result);
        }

        [HttpGet("GetOrderDetails")]
        public async Task<IActionResult> GetOrderDetails([FromQuery]string shopId, string orderId)
        {
            var shop = await _apiKeyAuthorize.GetShopAsync(shopId);
            if (shop == null)
            {
                return Unauthorized();
            }

            var result = await _ordersService.GetDetailsOrderByFilter(orderId);

            if (result != null)
            {
                var response = parseToJson(result, shop);
                if (response.ToString() == "Unauthorized")
                {
                    return Unauthorized();
                }
                else
                {
                    return GetResponseResult(response);
                }
            }
            else
            {
                return null;
            }
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("change-order-state")]
        public async Task<IActionResult> ChangeOrderState(string shopId, ChangeOrderStateCommand changeOrderStateCommand)
        {
            var shop = await _apiKeyAuthorize.GetShopAsync(shopId);
            if (shop == null)
            {
                return Unauthorized();
            }

            var result2 = await _ordersService.GetDetailsOrderByFilter(changeOrderStateCommand.orderId);

            var response = parseToJson(result2, shop);
            if (response.ToString() == "Unauthorized")
            {
                return Unauthorized();
            }
            else
            {
                var result = await _ordersService.ChangeStateOrder(changeOrderStateCommand);

                return GetResponseResult(result);
            }
        }

        private string ObtenerPedidoXML()
        {
            string pedidoXml = GetXml(AppDomain.CurrentDomain.BaseDirectory + @"Resources/Pedido1.xml");

            return pedidoXml;
        }

        private object parseToJson(string textXml, UserShopModel shop)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(textXml);

            XmlNode OrderNode = xmlDocument.SelectSingleNode("EstructuraPedido");

            if (OrderNode != null)
            {
                var attrTienda = OrderNode.FirstChild.Attributes["idTienda"];

                if (attrTienda != null && attrTienda.Value == shop.ShopId)
                {
                    var json = JsonConvert.SerializeXmlNode(OrderNode).Replace("@", "");
                    return JsonConvert.DeserializeObject(json);
                }
                else
                {
                    return "Unauthorized";
                }
            }
            else
            {
                return null;
            }
        }

        private string GetXml(string url)
        {
            using (XmlReader xr = XmlReader.Create(url, new XmlReaderSettings() { IgnoreWhitespace = true }))
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlWriter xw = XmlWriter.Create(sw))
                    {
                        xw.WriteNode(xr, false);
                    }
                    return sw.ToString();
                }
            }
        }

        private IActionResult GetResponseResult(dynamic dynamic)
         => dynamic == null ? NotFound() : Ok(dynamic);
    }
}
