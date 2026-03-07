using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace users_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase {
        private readonly IMongoClient _mongoClient;

        public UserController(IMongoClient mongoClient) {
            _mongoClient = mongoClient;
        }

        [HttpGet("/health")]
        public IActionResult GetHealth() {
            return Ok(new {status = "User API is healthy" });
        }

        [HttpGet("/status")]
        public IActionResult GetStatus() {
            return Ok(new { service = "Users", status = "Running" });
        }

        [HttpGet("/mongo-health")]
        public async Task<IActionResult> GetMongoHealth() {
            try
            {
                var db = _mongoClient.GetDatabase("admin");
                await db.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
                return Ok(new { status = "Connected", database = "MongoDB" });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new { status = "Disconnected", error = ex.Message });
            }
        }
    }
}
