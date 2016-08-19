using System;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Infra.Cc.Validators;
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

        public ValidatorResult Update(Task task)
        {
            var validation = new ValidatorResult();

            try
            {
                var toUpdate = Builders<Task>.Update
                    .Set(i => i.Title, task.Title)
                    .Set(i => i.Description, task.Description)
                    .Set(i => i.IsCompleted, task.IsCompleted);

                var updated = Context.MongoDatabase.GetCollection<Task>(TableName).UpdateOne(i => i.UserName == task.UserName && i.Id == task.Id, toUpdate);

                if (updated.MatchedCount == 0)
                {
                    validation.AddError(new ValidationError("Task was not found", ErroKeyEnum.NotFound));
                }
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

        public ValidatorResult Delete(Expression<Func<Task, bool>> query)
        {
            var validation = new ValidatorResult();

            try
            {
                var updated = Context.MongoDatabase.GetCollection<Task>(TableName).DeleteOne(query);

                if (updated.DeletedCount == 0)
                {
                    validation.AddError(new ValidationError("Task was not found", ErroKeyEnum.NotFound));
                }
            }
            catch (Exception ex)
            {
                validation.AddError(new ValidationError(ex.Message, ErroKeyEnum.InternalError));
            }

            return validation;
        }
    }
}