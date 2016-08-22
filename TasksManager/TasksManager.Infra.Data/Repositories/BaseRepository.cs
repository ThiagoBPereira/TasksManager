using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Infra.Cc.Validators;
using TasksManager.Infra.Data.Context;
using TasksManager.Infra.IoC.Resources;

namespace TasksManager.Infra.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly MongoDbContext Context;
        protected string TableName;
        protected IMongoCollection<TEntity> DbCollection => Context.MongoDatabase.GetCollection<TEntity>(TableName);


        public BaseRepository(MongoDbContext context)
        {
            Context = context;
        }

        public ValidatorResult CreateAsync(TEntity user)
        {
            var validationResult = new ValidatorResult();
            try
            {
                DbCollection.InsertOneAsync(user).Wait();
            }
            catch (AggregateException aggEx)
            {

                aggEx.Handle(x =>
                {
                    var mwx = x as MongoWriteException;
                    if (mwx != null)
                    {
                        validationResult.AddError(new ValidationError(mwx.WriteError.Message, ErroKeyEnum.DuplicateKey));
                        return true;
                    }

                    validationResult.AddError(new ValidationError(Resources.UnknownError, ErroKeyEnum.InternalError));
                    return false;
                });

                return validationResult;
            }
            catch (Exception ex)
            {
                validationResult.AddError(new ValidationError(ex.Message, ErroKeyEnum.InternalError));
            }
            return validationResult;
        }

        public IList<TEntity> Where(Expression<Func<TEntity, bool>> query)
        {
            return DbCollection.Find(query).ToList();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> query)
        {
            return DbCollection.Find(query).FirstOrDefault();
        }
    }
}