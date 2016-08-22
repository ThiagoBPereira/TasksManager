using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using TasksManager.Application.Interfaces;
using TasksManager.Infra.IoC.Resources;

namespace TasksManager.Api.Authorize
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserApp _userApp;

        public AuthorizationServerProvider(IUserApp userApp)
        {
            _userApp = userApp;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                var user = _userApp.GetUserByEmailAndPassword(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", string.Format(Resources.IsIncorrect, Resources.UsernameOrPassword));
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id));


                var principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch
            {
                context.SetError("invalid_grant", Resources.FailedToAuthenticate);
            }
        }
    }
}
