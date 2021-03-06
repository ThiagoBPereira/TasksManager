﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Infra.Cc.Validators;
using TasksManager.Infra.Data.Context;
using TasksManager.Infra.IoC.Resources;

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
            DbCollection.Indexes.CreateOne(new BsonDocument("Email", 1), new CreateIndexOptions { Unique = true, Sparse = true });
            DbCollection.Indexes.CreateOne(new BsonDocument("UserName", 1), new CreateIndexOptions { Unique = true, Sparse = true });
        }

        public TaskUser GetUserByEmailAndPassword(string email, string password)
        {
            return FirstOrDefault(i => i.Email == email && i.PasswordHash == password);
        }

        public TaskUser GetUserByUserNameAndPassword(string userName, string password)
        {
            return FirstOrDefault(i => i.UserName == userName && i.PasswordHash == password);
        }

        public ValidatorResult ChangePassword(string userId, string oldPasswordHash, string newPasswordHash)
        {
            var validation = new ValidatorResult();

            try
            {
                var toUpdate = Builders<TaskUser>.Update.Set(i => i.PasswordHash, newPasswordHash);
                var updated = DbCollection.UpdateOne(i => i.UserName == userId && i.PasswordHash == oldPasswordHash, toUpdate);

                if (updated.MatchedCount == 0)
                {
                    validation.AddError(new ValidationError(string.Format(Resources.IsIncorrect, Resources.UsernameOrPassword), ErroKeyEnum.NotFound));
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

                    validation.AddError(new ValidationError(Resources.UnknownError, ErroKeyEnum.InternalError));
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
