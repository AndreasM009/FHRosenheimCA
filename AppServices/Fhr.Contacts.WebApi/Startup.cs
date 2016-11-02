using System.Web.Http;
using Fhr.Contacts.WebApi.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Fhr.Contacts.WebApi.Startup))]

namespace Fhr.Contacts.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder aApp)
        {

            AutoMapper.Mapper.Initialize(cfg =>
                                         {
                                             cfg.CreateMap<Models.Contact, Contacts.Models.Contact>();
                                             cfg.CreateMap<Contacts.Models.Contact, Models.Contact>();
                                             cfg.CreateMap<Dto.ContactDto, Models.Contact>();
                                             cfg.CreateMap<Models.ContactSettings, Contacts.Models.ContactSettings>();
                                             cfg.CreateMap<Contacts.Models.ContactSettings, Models.ContactSettings>();
                                         });

            var config = new HttpConfiguration();

            // DI Container Unity
            UnityConfig.RegisterComponents(config);
            // Security
            AuthConfig.Configure(aApp);    
            // WebApi
            WebApiConfig.Configure(config);            
            // Enable CORS
            aApp.UseCors(CorsOptions.AllowAll);
            // Use WebApi
            aApp.UseWebApi(config);
            // Enable Swagger
            SwaggerConfig.Register(config);
        }
    }
}
