using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Fhr.Contacts.TransactionScript.Interfaces;
using Swashbuckle.Swagger.Annotations;

namespace Fhr.Contacts.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/contactservices/settings")]
    public class ContactSettingsController : AbstractApiController
    {
        private readonly IContactService mContactService;

        public ContactSettingsController(IContactService aContactService)
        {
            mContactService = aContactService;
        }

        [HttpGet]
        [ResponseType(typeof(Models.ContactSettings))]
        [SwaggerOperation("GetSettings")]
        [Route("")]
        public async Task<IHttpActionResult> GetAsync()
        {
            var s = await ProcessAsync(async uid => await mContactService.GetSettings(uid));

            return Ok(AutoMapper.Mapper.Map<Models.ContactSettings>(s));
        }

        [HttpPut]
        [ResponseType(typeof(Models.ContactSettings))]
        [SwaggerOperation("UpdateSettings")]
        [Route("")]
        public async Task<IHttpActionResult> UpdateAsync([FromBody]Models.ContactSettings aSettings)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = AutoMapper.Mapper.Map<Contacts.Models.ContactSettings>(aSettings);
            var result = await ProcessAsync(async uid => await mContactService.UpdateSettings(uid, model));
            return Ok(AutoMapper.Mapper.Map<Models.ContactSettings>(result));
        }
    }
}