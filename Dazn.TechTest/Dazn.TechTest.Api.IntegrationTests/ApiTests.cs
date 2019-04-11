using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Dazn.TechTest.Api.IntegrationTests
{
    public class ApiTests
    {
        [Test]
        public async Task CanStartApi()
        {
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var response = await client.GetAsync("test");

            response.EnsureSuccessStatusCode();
        }
    }
}