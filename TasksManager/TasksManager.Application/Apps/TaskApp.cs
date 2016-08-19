using System.Collections.Generic;
using System.Linq;
using TasksManager.Application.Interfaces;
using TasksManager.Application.ViewModels.Request;
using TasksManager.Application.ViewModels.Task;
using TasksManager.Domain.Interfaces.Services;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Application.Apps
{
    public class TaskApp : ITaskApp
    {
        private readonly ITaskService _taskService;

        public TaskApp(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public ValidatorResult Create(string username, TaskViewModelDetails task)
        {
            return _taskService.Create(new Domain.Entities.Task
            {
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                UserName = username
            });
        }


        public IEnumerable<TaskViewModelIndex> Get(string username)
        {
            var tasks = _taskService.GetByUserName(username)

                //Mapper, Here we can use Expressmapper or AutoMapper
                .Select(i => new TaskViewModelIndex
                {
                    TaskId = i.Id,
                    Title = i.Title,
                    IsCompleted = i.IsCompleted
                });

            return tasks;
        }

        public TaskViewModelDetails Get(UserNameAndTaskIdRequest request)
        {
            var task = _taskService.GetByUserNameAndTaskId(request.UserName, request.TaskId);
            if (task == null)
                return null;

            return new TaskViewModelDetails
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }
    }
}