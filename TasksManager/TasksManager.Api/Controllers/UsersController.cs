using System;
using System.Net;
using System.Web.Http;
using TasksManager.Application.Interfaces;
using TasksManager.Application.ViewModels.User;

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

        public IHttpActionResult Post([FromBody]UserViewModel userViewModel)
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
    }
}
