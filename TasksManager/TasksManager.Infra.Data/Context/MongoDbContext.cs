using System.Configuration;
using MongoDB.Driver;

namespace TasksManager.Infra.Data.Context
{
    public class MongoDbContext
    {
        //Instanciar controlador
        public readonly IMongoClient MongoClient;
        public readonly IMongoDatabase MongoDatabase;

        public MongoDbContext()
        {
            var mongoUrl = new MongoUrl(GetConnectionString("MongoDBConnectionString"));
            var settings = MongoClientSettings.FromUrl(mongoUrl);

            MongoClient = new MongoClient(settings);
            MongoDatabase = MongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        private static string GetConnectionString(string connectionString)
        {
            return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }
    }
}