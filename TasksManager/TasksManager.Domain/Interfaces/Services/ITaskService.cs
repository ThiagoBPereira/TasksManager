using System.Collections.Generic;
using TasksManager.Domain.Entities;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Interfaces.Services
{
    public interface ITaskService
    {
        IEnumerable<Task> GetByUserName(string username);
        ValidatorResult Create(Task task);
        Task GetByUserNameAndTaskId(string userName, string taskId);
        ValidatorResult Update(Task task);
        ValidatorResult Delete(string userName, string taskId);
    }
}