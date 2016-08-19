using System;
using System.Linq.Expressions;
using TasksManager.Domain.Entities;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IBaseRepository<Task>
    {
        ValidatorResult Update(Task task);
        ValidatorResult Delete(Expression<Func<Task, bool>> query);
    }
}