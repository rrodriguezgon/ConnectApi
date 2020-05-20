using OrderMailboxHub.Host.Authorization;
using OrderMailboxHub.Tests.IntegrationTests;
using Seedwork.Test;
using Xunit;

namespace OrderMailboxHub.Tests.Integration
{
    public class ApiKeyValueTests : BaseTest<TestStartup>
    {
        private const string BASE_PATH = "api/loglevel";

        [Fact]
        public virtual void Check_Shop_ID_From_Api_Key()
        {
            // https://encode-decode.com/aes-128-ecb-encrypt-online/
            string apiKeyText = "548bcdaa-ac21-497d-96ed-d04e7d48a630:34";
            string apiKey = "ghluhtfWcJWgzUYtAKhdmerdUQNCkiknmDVYb9dUHg5mpgYD9hbwm4qo7VOFBh+k";
            string encryptionKey = "85AD1765D238E592";
            string value = "34";

            ApiKeyValue apiKeyValue = new ApiKeyValue(apiKey, encryptionKey);

            Assert.Equal(value, apiKeyValue.CountryId);
            Assert.Equal(apiKeyText, apiKeyValue.ToString());
        }
    }
}
