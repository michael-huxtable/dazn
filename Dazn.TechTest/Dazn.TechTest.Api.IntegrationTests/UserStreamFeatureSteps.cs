using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using StackExchange.Redis;
using TechTalk.SpecFlow;

namespace Dazn.TechTest.Api.IntegrationTests
{
    [Binding]
    public class UserStreamFeatureSteps
    {
        private HttpClient _client;
        
        [BeforeScenario]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }

        [Given(@"user with id (.*) is not streaming video")]
        public void GivenUserWithIdIsNotStreamingVideo(int userId)
        {
            
        }
        
        [When(@"the stream count is updated (.*) time\(s\)")]
        public void WhenTheStreamCountIsUpdatedTimes(int times)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I should get a bad request response with exceeded limit message")]
        public void ThenIShouldGetABadRequestResponseWithExceededLimitMessage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should get a response with count of (.*)")]
        public void ThenIShouldGetAResponseWithCountOf(int count)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
