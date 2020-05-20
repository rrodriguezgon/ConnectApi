using OrderMailboxHub.Tests.IntegrationTests;
using Seedwork.Test;
using System.Threading.Tasks;
using Xunit;

namespace OrderMailboxHub.Tests.Integration
{
    public class VersionControllerTests
        : BaseTest<TestStartup>
    {
        private const string BASE_PATH = "api/version";

        [Fact]
        public virtual async Task Get()
        {
            var response = await Client.GetAsync($"{BASE_PATH}");

            var responseMessage = response.EnsureSuccessStatusCode();

            Assert.Contains(responseMessage.StatusCode, ExpectedStatues);
        }
    }
}
