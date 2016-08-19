﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Domain.Entities
{
    public class Task
    {
        public Task()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonId]
        public string Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string UserName { get; set; }

        [BsonIgnore]
        public ValidatorResult ValidatorResult { get; set; }

        public bool IsValid()
        {
            ValidatorResult = new ValidatorResult();

            //Verify Title
            VerifyTitle();

            return ValidatorResult.IsSuccess;
        }


        private void VerifyTitle()
        {
            if (string.IsNullOrEmpty(Title.Trim()))
            {
                ValidatorResult.AddError(new ValidationError("Title can't be empty", ErroKeyEnum.EmptyError));
            }
        }
    }
}