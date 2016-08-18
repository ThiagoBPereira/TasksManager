using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        ValidatorResult CreateAsync(TEntity user);
        IList<TEntity> Where(Expression<Func<TEntity, bool>> query);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> query);
        IList<TEntity> GetAll();
    }
}