using Microsoft.AspNetCore.Authentication;

namespace OrderMailboxHub.Host.Authorization
{
    public class ApiKeyOptions : AuthenticationSchemeOptions
    {
        public string EncriptionKey { get; set; }

        public ApiKeyOptions() { }
    }
}
