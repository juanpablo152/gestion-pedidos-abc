using Microsoft.Extensions.Options;
using MongoDB.Driver;
using payment_api.Application.Interfaces;
using payment_api.Domain.Entities;
using payment_api.Infrastructure.DB;

namespace payment_api.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoCollection<Payment> _paymentsCollection;

        public PaymentRepository(IOptions<MongoConfig> mongoConfig)
        {
            var mongoClient = new MongoClient(mongoConfig.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoConfig.Value.DatabaseName);
            _paymentsCollection = mongoDatabase.GetCollection<Payment>(mongoConfig.Value.PaymentsCollectionName);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(string orderId)
        {
            return await _paymentsCollection.Find(p => p.OrderId == orderId).ToListAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(string id)
        {
            return await _paymentsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _paymentsCollection.InsertOneAsync(payment);
        }

        public async Task UpdatePaymentAsync(string id, Payment payment)
        {
            await _paymentsCollection.ReplaceOneAsync(p => p.Id == id, payment);
        }

        public async Task DeletePaymentByIdAsync(string id)
        {
            await _paymentsCollection.DeleteOneAsync(p => p.Id == id);
        }
    }
}
