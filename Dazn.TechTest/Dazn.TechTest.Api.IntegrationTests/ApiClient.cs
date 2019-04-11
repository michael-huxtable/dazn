using System.Net.Http;
using System.Threading.Tasks;

namespace Dazn.TechTest.Api.IntegrationTests
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public Task<HttpResponseMessage> GetStreamCount(int userId)
        {
            return _client.GetAsync(GetUserStreamUri(userId));
        }

        public Task<HttpResponseMessage> ResetStreamCount(int userId)
        {
            return _client.DeleteAsync(GetUserStreamUri(userId));
        }

        private static string GetUserStreamUri(int userId)
        {
            return $"user/{userId}/stream";
        }
    }
}