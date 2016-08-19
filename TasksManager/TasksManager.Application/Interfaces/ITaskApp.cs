using System.Collections.Generic;
using TasksManager.Application.ViewModels.Request;
using TasksManager.Application.ViewModels.Task;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Application.Interfaces
{
    public interface ITaskApp
    {
        ValidatorResult Create(string username, TaskViewModelDetails task);
        IEnumerable<TaskViewModelIndex> Get(string username);
        TaskViewModelDetails Get(UserNameAndTaskIdRequest request);
        ValidatorResult Update(string username, string taskId, TaskViewModelDetails task);
    }
}