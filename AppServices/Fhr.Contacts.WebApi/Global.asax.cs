using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;

namespace Fhr.Contacts.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
#if DEBUG
            TelemetryConfiguration.Active.DisableTelemetry = true;
#endif

#if TEST
            TelemetryConfiguration.Active.DisableTelemetry = true;
#endif
        }
    }
}
