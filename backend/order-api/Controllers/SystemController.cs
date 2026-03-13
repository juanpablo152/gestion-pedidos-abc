using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace order_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly IMongoClient _mongoClient;

        public SystemController(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet("/health")]
        public IActionResult GetHealth()
        {
            return Ok(new { status = "Orders API is healthy" });
        }

        [HttpGet("/status")]
        public IActionResult GetStatus()
        {
            return Ok(new { service = "Orders", status = "Running" });
        }

        [HttpGet("/mongo-health")]
        public async Task<IActionResult> GetMongoHealth()
        {
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
