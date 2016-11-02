using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Fhr.Contacts.TransactionScript.Interfaces;
using Swashbuckle.Swagger.Annotations;

namespace Fhr.Contacts.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/contactservices/contacts")]
    public class ContactController : AbstractApiController
    {
        private readonly IContactService mContactService;        

        public ContactController(IContactService aContactService)
        {
            mContactService = aContactService;
        }

        [HttpGet]
        [ResponseType(typeof(List<Models.Contact>))]
        [SwaggerOperation("GetAll")]
        [Route("")]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var result = await ProcessAsync(async uid =>
                                                  {
                                                      var models = await mContactService.GetAll(uid);
                                                      return models.Select(AutoMapper.Mapper.Map<Models.Contact>).ToList();
                                                  });

            return Ok(result);
        }

        [HttpGet]
        [ResponseType(typeof(Models.Contact))]
        [SwaggerOperation("GetById")]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> GetByIdAsync(Guid id)
        {
            var result = await ProcessAsync(async uid =>
            {
                var contact = await mContactService.GetById(uid, id);
                return AutoMapper.Mapper.Map<Models.Contact>(contact);
            });

            if (null == result)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ResponseType(typeof(Models.Contact))]
        [SwaggerOperation("Add")]
        [Route("")]
        public async Task<IHttpActionResult> AddAsync([FromBody]Models.Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = AutoMapper.Mapper.Map<Contacts.Models.Contact>(contact);

            var result = await ProcessAsync(async uid => await mContactService.Add(uid, model, contact.Image));            

            return Ok(AutoMapper.Mapper.Map<Models.Contact>(result));
        }        

        [HttpPut]
        [ResponseType(typeof(Models.Contact))]
        [SwaggerOperation("Update")]
        [Route("")]
        public async Task<IHttpActionResult> UpdateAsync([FromBody]Models.Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = AutoMapper.Mapper.Map<Contacts.Models.Contact>(contact);

            var result = await ProcessAsync(async uid => await mContactService.Update(uid, model, contact.Image));

            return Ok(AutoMapper.Mapper.Map<Models.Contact>(result));
        }

        [HttpDelete]
        [SwaggerOperation("Delete")]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            await ProcessAsync(async uid =>await mContactService.Delete(uid, id));
            return Ok();
        }        
    }
}
