using System.Collections.Generic;
using TasksManager.Domain.Entities;
using TasksManager.Domain.Interfaces.Repositories;
using TasksManager.Domain.Interfaces.Services;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<Task> GetByUserName(string username)
        {
            return _taskRepository.Where(i => i.UserName == username);
        }

        public ValidatorResult Create(Task task)
        {
            if (task.IsValid())
            {
                return _taskRepository.CreateAsync(task);
            }

            return task.ValidatorResult;
        }

        public Task GetByUserNameAndTaskId(string userName, string taskId)
        {
            return _taskRepository.FirstOrDefault(i => i.UserName == userName && i.Id == taskId);
        }
    }
}