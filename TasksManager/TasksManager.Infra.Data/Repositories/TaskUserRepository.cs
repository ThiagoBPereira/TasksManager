using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Infra.Cc.Validators;
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
            Context.MongoDatabase.GetCollection<TaskUser>(TableName).Indexes.CreateOne(new BsonDocument("UserName", 1), new CreateIndexOptions { Unique = true, Sparse = true });
        }

        public TaskUser GetUserByEmailAndPassword(string email, string password)
        {
            return FirstOrDefault(i => i.Email == email && i.PasswordHash == password);
        }

        public TaskUser GetUserByUserNameAndPassword(string userName, string password)
        {
            return FirstOrDefault(i => i.UserName == userName && i.PasswordHash == password);
        }

        public ValidatorResult ChangePassword(string userId, string passwordHash)
        {
            var validation = new ValidatorResult();

            try
            {
                var filter = Builders<TaskUser>.Filter.Eq(i => i.UserName, userId);
                var update = Builders<TaskUser>.Update.Set(i => i.PasswordHash, passwordHash);
                Context.MongoDatabase.GetCollection<TaskUser>(TableName).UpdateOne(filter, update);
            }
            catch (AggregateException aggEx)
            {

                aggEx.Handle(x =>
                {
                    var mwx = x as MongoWriteException;
                    if (mwx != null)
                    {
                        validation.AddError(new ValidationError(mwx.WriteError.Message, ErroKeyEnum.DuplicateKey));
                        return true;
                    }

                    validation.AddError(new ValidationError("Unknown error", ErroKeyEnum.InternalError));
                    return false;
                });

                return validation;
            }
            catch (Exception ex)
            {
                validation.AddError(new ValidationError(ex.Message, ErroKeyEnum.InternalError));
            }
            return validation;
        }
    }
}
