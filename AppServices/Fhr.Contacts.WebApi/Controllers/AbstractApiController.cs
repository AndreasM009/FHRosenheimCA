using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fhr.Contacts.WebApi.Controllers
{
    [Authorize]
    public abstract class AbstractApiController : ApiController
    {
        protected async Task ProcessAsync(Func<Guid, Task> aFunc)
        {
            var userId = Guid.Parse(ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            await aFunc(userId);
        }

        protected async Task<TResult> ProcessAsync<TResult>(Func<Guid, Task<TResult>> aFunc)
        {
            var userId = Guid.Parse(ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            return await aFunc(userId);
        }
    }
}