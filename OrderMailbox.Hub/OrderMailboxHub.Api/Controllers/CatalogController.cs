using OrderMailboxHub.Application.Dtos.Catalog;
using Microsoft.AspNetCore.Mvc;
using Seedwork.CrossCutting.MVC.Dtos;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using OrderMailboxHub.Application.Services.TPGESCOM;
using OrderMailboxHub.Application.Services.Common;
using Microsoft.AspNetCore.Authorization;

namespace OrderMailboxHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "apikey")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        private readonly IApiKeyAuthorize _apiKeyAuthorize;

        public CatalogController(ICatalogService catalogService, IApiKeyAuthorize apiKeyAuthorize)
        {
            _catalogService = catalogService;
            _apiKeyAuthorize = apiKeyAuthorize;
        }

        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("products")]
        public async Task<IActionResult> GetCatalog([FromQuery]string idShop)
        {
            var shop = await _apiKeyAuthorize.GetShopAsync(idShop);
            if (shop == null)
            {
                return Unauthorized();
            }

            CatalogFilterDto catalogFilterDto = new CatalogFilterDto();
            catalogFilterDto.idShop = idShop;
            catalogFilterDto.countryId = shop.CountryId;

            var result = await _catalogService.GetProductsByFilter(catalogFilterDto);

            return GetResponseResult(result);
        }

        [ProducesResponseType(typeof(PromotionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("promotions")]
        public async Task<IActionResult> GetPromotions([FromQuery]string idShop)
        {
            var shop = await _apiKeyAuthorize.GetShopAsync(idShop);
            if (shop == null)
            {
                return Unauthorized();
            }

            PromotionFilterDto promotionFilterDto = new PromotionFilterDto();
            promotionFilterDto.idShop = idShop;
            promotionFilterDto.countryId = shop.CountryId;

            var result = await _catalogService.GetPromotionsByFilter(promotionFilterDto);

            return GetResponseResult(result);
        }

        private IActionResult GetResponseResult(dynamic dynamic)
         => dynamic == null ? NotFound() : Ok(dynamic);
    }
}