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
            MongoClient = new MongoClient("mongodb://localhost:27017");
            MongoDatabase = MongoClient.GetDatabase("mytasks");
        }
    }
}