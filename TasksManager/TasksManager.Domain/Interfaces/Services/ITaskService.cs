﻿using System.Collections.Generic;
using TasksManager.Domain.Entities;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Interfaces.Services
{
    public interface ITaskService
    {
        IEnumerable<Entities.Task> GetByUserName(string username);
        ValidatorResult Create(Task task);
        Entities.Task GetByUserNameAndTaskId(string userName, string taskId);
    }
}