using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;

namespace Dazn.TechTest.Api.IntegrationTests
{
    [Binding]
    public class UserStreamsFeatureSteps
    {
        private HttpClient _client;
        private readonly List<HttpResponseMessage> _responses = new List<HttpResponseMessage>();
        private HttpResponseMessage LastResponse => _responses.Last();

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
        }
        
        [When(@"the stream count is updated (.*) time\(s\)")]
        public async Task WhenTheStreamCountIsUpdatedTimes(int times)
        {
            for (int i = 0; i < times; i++)
            {
                var response = await _client.GetAsync($"user/{_userId}/stream");
                _responses.Add(response);
            }
        }

        [When(@"the stream count is updated (.*) times in parallel")]
        public async Task WhenTheStreamCountIsUpdatedTimesInParallel(int times)
        {
            var tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < times; i++)
            {
                tasks.Add(_client.GetAsync($"user/{_userId}/stream"));
            }

            await Task.WhenAll(tasks);
            _responses.AddRange(tasks.Select(t => t.Result));
        }

        [Then(@"I should get a bad request response with exceeded limit message for the last (.*) requests")]
        public void ThenIShouldGetABadRequestResponseWithExceededLimitMessageForTheLastRequests(int requestCount)
        {
            var lastResponses = _responses.TakeLast(requestCount);

            using (new AssertionScope())
            {
                foreach (var response in lastResponses)
                {
                    response.StatusCode.Should().Be(400);
                }
            }
        }

        [Then(@"the status code of the last response should be (.*)")]
        public void ThenTheStatusCodeOfTheLastResponseShouldBe(int statusCode)
        {
            LastResponse.StatusCode.Should().Be(statusCode);
        }
        
        [Then(@"the content of the last response should be (.*)")]
        public async Task ThenTheContentOfTheLastResponseShouldBe(int expectedCount)
        {
            int count = await LastResponse.Content.ReadAsAsync<int>();
            count.Should().Be(expectedCount);
        }

        [Then(@"I should get a bad request response with exceeded limit message for (.*) requests")]
        public void ThenIShouldGetABadRequestResponseWithExceededLimitMessageForRequests(int expectedCount)
        {
            var badRequests = _responses.Where(r => r.StatusCode == HttpStatusCode.BadRequest);
            badRequests.Count().Should().Be(expectedCount);
        }
    }
}
