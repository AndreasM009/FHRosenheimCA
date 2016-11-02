using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fhr.Contacts.WebApi
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            // API Routes            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/contactservices/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Remove XmlFormatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // JSON Serialization
            JsonSerializerSettings settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
        }
    }
}
