using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Infra.Cc.Validators;
using TasksManager.Infra.Data.Context;

namespace TasksManager.Infra.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly MongoDbContext Context;
        protected string TableName;

        public BaseRepository(MongoDbContext context)
        {
            Context = context;
        }

        public ValidatorResult CreateAsync(TEntity user)
        {
            var validationResult = new ValidatorResult();
            try
            {
                Context.MongoDatabase.GetCollection<TEntity>(TableName).InsertOneAsync(user).Wait();
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

                    validationResult.AddError(new ValidationError("Unknown error", ErroKeyEnum.InternalError));
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
            return Context.MongoDatabase.GetCollection<TEntity>(TableName).Find(query).ToList();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> query)
        {
            return Context.MongoDatabase.GetCollection<TEntity>(TableName).Find(query).FirstOrDefault();
        }

        public IList<TEntity> GetAll()
        {
            return Context.MongoDatabase.GetCollection<TEntity>(TableName).AsQueryable().ToList();
        }
    }
}