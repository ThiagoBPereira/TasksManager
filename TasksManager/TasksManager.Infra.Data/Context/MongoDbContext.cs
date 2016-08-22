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
            MongoClient = new MongoClient(ConfigurationManager.AppSettings["ConnectionString"]);
            MongoDatabase = MongoClient.GetDatabase(ConfigurationManager.AppSettings["Database"]);
        }
    }
}