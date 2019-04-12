using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Dazn.TechTest.Api.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public UserController(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        [HttpPost]
        [Route("{userId}/stream")]
        public async Task<ActionResult> IncrementStreamCount(int userId)
        {
            //Do not validate user is logged in / authenticated,
            //presumed not to be in scope.
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string key = GetRedisStreamKey(userId);

            const int maximumStreams = 3;

            var response = (int) await db.ScriptEvaluateAsync(@"
                local result = redis.call('incr', KEYS[1])
                local maximum = tonumber(ARGV[1]);
                if result > maximum then
                    redis.call('set', KEYS[1], maximum)
                end
                return result", new RedisKey[] { key }, new RedisValue[] { maximumStreams });

            if (response > maximumStreams)
            {
                return BadRequest("Maximum stream count exceeded");
            }

            return Ok(response);
        }
        
        [HttpDelete]
        [Route(("{userId}/stream"))]
        public async Task<OkResult> ResetStreamCount(int userId)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string key = GetRedisStreamKey(userId);

            await db.KeyDeleteAsync(key);

            return Ok();
        }

        private string GetRedisStreamKey(int userId) => $"user:{userId}:streamCount";
    }
}
