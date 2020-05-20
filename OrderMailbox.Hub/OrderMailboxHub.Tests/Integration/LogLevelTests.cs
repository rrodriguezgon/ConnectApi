using OrderMailboxHub.Tests.IntegrationTests;
using Seedwork.Test;
using System.Threading.Tasks;
using Xunit;

namespace OrderMailboxHub.Tests.Integration
{
    public class LogLevelTests
        : BaseTest<TestStartup>
    {
        private const string BASE_PATH = "api/loglevel";

        [Fact]
        public virtual async Task LogLevelCurrentTest()
        {
            var response = await Client.GetAsync($"{BASE_PATH}/current");

            var responseMessage = response.EnsureSuccessStatusCode();

            Assert.Contains(responseMessage.StatusCode, ExpectedStatues);
        }

        [Fact]
        public virtual async Task LogLevelVerboseTest()
        {
            var response = await Client.PostAsync($"{BASE_PATH}/verbose", null);

            var responseMessage = response.EnsureSuccessStatusCode();

            Assert.Contains(responseMessage.StatusCode, ExpectedStatues);
        }
    }
}
