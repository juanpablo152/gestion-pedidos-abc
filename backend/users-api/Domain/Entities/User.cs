using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace users_api.Domain.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
