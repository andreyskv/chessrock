using System;
using System.Security.Claims;
using System.Web.Http;
using App.Identity;
using App.ViewModels;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using App.Common;

namespace App.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        [HttpPost, Route("Token")]
        public IHttpActionResult Token(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequestError(ModelState);
            }
            
            ClaimsIdentity identity;

            if (!_loginProvider.ValidateCredentials(login.UserName, login.Password, out identity))
            {             
                return BadRequest("Incorrect user or password");
            }

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
            
            return Ok(new LoginAccessViewModel
            {
                UserName = login.UserName,
                AccessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket)
            });
        }

        [Authorize]
        [HttpGet, Route("Profile")]
        public IHttpActionResult Profile()
        {            
            return Ok(new AccountProfileViewModel
            {
                UserName = User.Identity.Name
            });
        }

        /// <summary>
        /// Use this action to test authentication
        /// </summary>
        /// <returns>status code 200 if the user is authenticated, otherwise status code 401</returns>
        [Authorize]
        [HttpGet, Route("Ping")]
        public IHttpActionResult Ping()
        {            
            return Ok();
        }

        public AccountController(ILoginProvider loginProvider)
        {         
            _loginProvider = loginProvider;
        }

        private readonly ILoginProvider _loginProvider;       
    }
}
