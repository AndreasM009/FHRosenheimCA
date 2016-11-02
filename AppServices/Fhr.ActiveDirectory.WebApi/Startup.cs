using System.Web.Http;
using Fhr.ActiveDirectory.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Fhr.ActiveDirectory.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder aApp)
        {
            
            var config = new HttpConfiguration();
            
            // Security
            AuthConfig.Configure(aApp);    
            // WebApi
            WebApiConfig.Register(config);            
            // Enable CORS
            aApp.UseCors(CorsOptions.AllowAll);
            // Use WebApi
            aApp.UseWebApi(config);
            // Enable Swagger
            SwaggerConfig.Register(config);
        }
    }
}
