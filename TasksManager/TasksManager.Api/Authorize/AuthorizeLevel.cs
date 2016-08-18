﻿using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TasksManager.Api.Authorize
{
    public class AuthorizeRouteByClaim : AuthorizeAttribute
    {
        private readonly string _fieldRoute;
        private readonly string _claimType;
        public AuthorizeRouteByClaim(string fieldRoute, string claimType)
        {
            _fieldRoute = fieldRoute;
            _claimType = claimType;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                return false;
            }

            //Get user request
            var identity = (ClaimsIdentity)actionContext.RequestContext.Principal.Identity;

            //Get Identity Claim
            var userKey = identity.FindFirst(i => i.Type == _claimType);
            if (userKey == null)
            {
                return false;
            }

            //Get Route Field
            var routeUserKey = actionContext.RequestContext.RouteData.Values.FirstOrDefault(i => i.Key == _fieldRoute);

            //Compare
            return userKey.Value == (string)routeUserKey.Value;
        }
    }
}