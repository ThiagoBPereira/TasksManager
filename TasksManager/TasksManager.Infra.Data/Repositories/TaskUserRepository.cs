using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Infra.Data.Context;

namespace TasksManager.Infra.Data.Repositories
{
    public class TaskUserRepository : BaseRepository<TaskUser>, ITaskUserRepository
    {
        public TaskUserRepository(MongoDbContext context) : base(context)
        {
            TableName = "users";

            if (BsonClassMap.IsClassMapRegistered(typeof(TaskUser)))
                return;

            BsonClassMap.RegisterClassMap<TaskUser>();
            CreatingIndex();
        }

        private void CreatingIndex()
        {
            Context.MongoDatabase.GetCollection<TaskUser>(TableName).Indexes.CreateOne(new BsonDocument("Email", 1), new CreateIndexOptions { Unique = true, Sparse = true });
        }

        public TaskUser GetUserByEmailAndPassword(TaskUser taskUser)
        {
            return FirstOrDefault(i => i.Email == taskUser.Email && i.PasswordHash == taskUser.PasswordHash);
        }
    }
}
