using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using TasksManager.Api.Authorize;
using TasksManager.Application.Interfaces;
using TasksManager.Application.ViewModels.Request;
using TasksManager.Application.ViewModels.User;
using TasksManager.Infra.Cc.Validators;

namespace TasksManager.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IUserApp _userApp;

        public UsersController(IUserApp userApp)
        {
            _userApp = userApp;
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]UserViewModelCreate userViewModel)
        {
            try
            {
                var validation = _userApp.Create(userViewModel);

                if (validation.IsSuccess)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, validation);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{userName}/ChangePassword")]
        [AuthorizeRouteByClaim("userName", ClaimTypes.Name)]
        public IHttpActionResult Put([FromUri]UserNameRequest userRequest, [FromBody]UserChangePasswordViewModel userViewModel)
        {
            try
            {
                userViewModel.UserName = userRequest.UserName;

                var validation = _userApp.ChangePassword(userViewModel);

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
