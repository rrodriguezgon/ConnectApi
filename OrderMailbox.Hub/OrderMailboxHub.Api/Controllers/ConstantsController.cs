using Microsoft.AspNetCore.Mvc;
using Seedwork.CrossCutting.MVC.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using OrderMailboxHub.Application.Dtos.Constants;
using OrderMailboxHub.Application.Services.TPGESCOM;
using Microsoft.AspNetCore.Authorization;
using OrderMailboxHub.Application.Services.Common;

namespace OrderMailboxHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "apikey")]
    public class ConstantsController : ControllerBase
    {
        private readonly IConstantsService _constantsService;

        private readonly IApiKeyAuthorize _apiKeyAuthorize;

        public ConstantsController(IConstantsService constantsService, IApiKeyAuthorize apiKeyAuthorize)
        {
            _constantsService = constantsService;
            _apiKeyAuthorize = apiKeyAuthorize;
        }

        [ProducesResponseType(typeof(ConstantsDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet()]
        public async Task<IActionResult> GetConstants()
        {
            string countryId = _apiKeyAuthorize.GetCountryId();

            var result = await _constantsService.GetAllConstants(countryId);

            return GetResponseResult(result);
        }

        private IActionResult GetResponseResult(dynamic dynamic)
        => dynamic == null ? NotFound() : Ok(dynamic);
    }
}
