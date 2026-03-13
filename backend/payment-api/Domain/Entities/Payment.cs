using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace payment_api.Domain.Entities
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; } = null!;

        public decimal Amount { get; set; }

        [BsonRepresentation(BsonType.String)]
        public PaymentMethod Method { get; set; } = PaymentMethod.Cash;

        [BsonRepresentation(BsonType.String)]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
}
