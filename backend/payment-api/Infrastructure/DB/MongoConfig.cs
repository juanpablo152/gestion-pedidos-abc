namespace payment_api.Infrastructure.DB
{
    public class MongoConfig
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string PaymentsCollectionName { get; set; } = null!;
    }
}
