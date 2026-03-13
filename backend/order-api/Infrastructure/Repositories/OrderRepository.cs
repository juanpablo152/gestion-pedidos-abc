using Microsoft.Extensions.Options;
using MongoDB.Driver;
using order_api.Application.Interfaces;
using order_api.Domain.Entities;
using order_api.Infrastructure.DB;

namespace order_api.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderRepository(IOptions<MongoConfig> mongoConfig)
        {
            var mongoClient = new MongoClient(mongoConfig.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoConfig.Value.DatabaseName);
            _ordersCollection = mongoDatabase.GetCollection<Order>(mongoConfig.Value.OrdersCollectionName);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _ordersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _ordersCollection.Find(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            return await _ordersCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _ordersCollection.InsertOneAsync(order);
        }

        public async Task UpdateOrderAsync(string id, Order order)
        {
            await _ordersCollection.ReplaceOneAsync(o => o.Id == id, order);
        }

        public async Task DeleteOrderByIdAsync(string id)
        {
            await _ordersCollection.DeleteOneAsync(o => o.Id == id);
        }
    }
}
