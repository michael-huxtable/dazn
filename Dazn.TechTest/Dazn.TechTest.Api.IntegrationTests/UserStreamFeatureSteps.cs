using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;

namespace Dazn.TechTest.Api.IntegrationTests
{
    [Binding]
    public class UserStreamFeatureSteps
    {
        private HttpClient _client;
        private HttpResponseMessage _lastResponse;
        private int _userId;

        [BeforeScenario]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }

        [Given(@"user with id (.*) is not streaming video")]
        public async Task GivenUserWithIdIsNotStreamingVideo(int userId)
        {
            _userId = userId;

            var response = await _client.DeleteAsync($"user/{userId}/stream");
            response.EnsureSuccessStatusCode();
            _lastResponse = response;
        }
        
        [When(@"the stream count is updated (.*) time\(s\)")]
        public async Task WhenTheStreamCountIsUpdatedTimes(int times)
        {
            for (int i = 0; i < times; i++)
            {
                var response = await _client.GetAsync($"user/{_userId}/stream");
                _lastResponse = response;
            }
        }

        [Then(@"I should get a bad request response with exceeded limit message")]
        public void ThenIShouldGetABadRequestResponseWithExceededLimitMessage()
        {
            _lastResponse.StatusCode.Should().Be(400);
        }
        
        [Then(@"I should get a response with count of (.*)")]
        public async Task ThenIShouldGetAResponseWithCountOf(int expectedCount)
        {
            _lastResponse.StatusCode.Should().Be(200);
            int count = await _lastResponse.Content.ReadAsAsync<int>();

            count.Should().Be(expectedCount);
        }
    }
}
