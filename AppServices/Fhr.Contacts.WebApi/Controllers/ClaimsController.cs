using System;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using Fhr.Contacts.WebApi.Models;
using Swashbuckle.Swagger.Annotations;

namespace Fhr.Contacts.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/contactservices/claims")]
    public class ClaimsController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(Claims))]
        [SwaggerOperation("Get")]
        [Route("")]
        public IHttpActionResult Get()
        {
            var claims = new Claims
                        {
                            Id = Guid.Parse(ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value),
                            GivenName = ClaimsPrincipal.Current.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value,
                            Surename = ClaimsPrincipal.Current.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value,
                            Email = ClaimsPrincipal.Current.FindFirst("emails").Value,
                            DisplayName = ClaimsPrincipal.Current.FindFirst("name").Value
                        };

            return Ok(claims);
        }
    }
}