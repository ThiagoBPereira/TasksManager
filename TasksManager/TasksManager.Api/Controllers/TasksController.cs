using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using TasksManager.Api.Authorize;
using TasksManager.Application.Interfaces;
using TasksManager.Application.ViewModels.Request;
using TasksManager.Application.ViewModels.Task;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Api.Controllers
{
    public class TasksController : ApiController
    {
        private readonly ITaskApp _taskApp;

        public TasksController(ITaskApp taskApp)
        {
            _taskApp = taskApp;
        }

        [HttpPost]
        [Route("api/Users/{username}/tasks")]
        [AuthorizeRouteByClaim("userName", ClaimTypes.Name)]
        public IHttpActionResult Post([FromUri]UserNameRequest userRequest, [FromBody]TaskViewModelDetails task)
        {
            try
            {
                var validation = _taskApp.Create(userRequest.UserName, task);

                if (validation.IsSuccess)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, validation);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Users/{username}/tasks")]
        public IHttpActionResult Get([FromUri]UserNameRequest userRequest)
        {
            try
            {
                var tasks = _taskApp.Get(userRequest.UserName);

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Users/{username}/tasks/{taskId}")]
        public IHttpActionResult Get([FromUri]UserNameAndTaskIdRequest userRequest)
        {
            try
            {
                var task = _taskApp.Get(userRequest);

                if (task != null)
                    return Ok(task);

                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Users/{username}/tasks/{taskId}")]
        [AuthorizeRouteByClaim("userName", ClaimTypes.Name)]
        public IHttpActionResult Put([FromUri]UserNameAndTaskIdRequest userRequest, [FromBody]TaskViewModelDetails task)
        {
            try
            {
                var validation = _taskApp.Update(userRequest.UserName, userRequest.TaskId, task);

                if (validation.IsSuccess)
                    return Ok();

                if (validation.Errors.Any(i => i.ErrorKey == ErroKeyEnum.NotFound))
                    return NotFound();

                return Content(HttpStatusCode.BadRequest, validation);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
