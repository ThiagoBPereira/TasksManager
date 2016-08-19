using MongoDB.Bson.Serialization;
using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Infra.Data.Context;

namespace TasksManager.Infra.Data.Repositories
{
    public class TaskRepository : BaseRepository<Task>, ITaskRepository
    {
        public TaskRepository(MongoDbContext context) : base(context)
        {
            TableName = "tasks";

            if (BsonClassMap.IsClassMapRegistered(typeof(Task)))
                return;

            BsonClassMap.RegisterClassMap<Task>();
        }
    }
}