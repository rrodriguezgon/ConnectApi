using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OrderMailboxHub.Host.Authorization
{
    public class ApiKeyHandler : AuthenticationHandler<ApiKeyOptions>
    {
        private const string scheme = "apikey";
        private const string headerName = "X-Api-Key";
        private ApiKeyOptions apiKeyOptions;        

        public ApiKeyHandler(
            IOptionsMonitor<ApiKeyOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            apiKeyOptions = options.CurrentValue;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = GetApiKeyHeaderValue();
            if (string.IsNullOrEmpty(apiKey))
            {
                return AuthenticateResult.Fail($"Incorrect '{headerName}' header.");
            }

            ApiKeyValue apiKeyValue;
            if (!TryGetApiKeyValue(apiKey, apiKeyOptions.EncriptionKey, out apiKeyValue))
            {
                return AuthenticateResult.Fail($"{headerName} is not valid.");
            }

            var identity = new ClaimsIdentity("api");
            // Add User Id:
            identity.AddClaim(new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", 
                apiKeyValue.ObjectIdentifier));

            // Add CountryId claims:
            identity.AddClaim(new Claim("CountryId", apiKeyValue.CountryId));            

            return await Task.FromResult<AuthenticateResult>(AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(identity), headerName)));
        }

        private string GetApiKeyHeaderValue()
        {
            StringValues values;
            if (Context.Request.Headers.TryGetValue(headerName, out values))
            {
                return values.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private bool TryGetApiKeyValue(string apiKey, string encryptionKey, out ApiKeyValue apiKeyValue)
        {
            try
            {
                apiKeyValue = new ApiKeyValue(apiKey, encryptionKey);
                return true;
            }
            catch
            {
                apiKeyValue = null;
                return false;
            }
        }
    }
}
