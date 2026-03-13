using Microsoft.Extensions.Options;
using MongoDB.Driver;
using users_api.Application.Interfaces;
using users_api.Domain.Entities;
using users_api.Infrastructure.DB;

namespace users_api.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(IOptions<MongoConfig> mongoConfig)
        {
            var mongoClient = new MongoClient(mongoConfig.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoConfig.Value.DatabaseName);
            _usersCollection = mongoDatabase.GetCollection<User>(mongoConfig.Value.UsersCollectionName);
        }

        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, user);
        }
    }
}
