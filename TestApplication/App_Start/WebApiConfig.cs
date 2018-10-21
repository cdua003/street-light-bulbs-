using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TestApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var json = configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API configuration and services
            Bootstrapper bootstrapper = new Bootstrapper(new UnityContainer());
            bootstrapper.SetupContainerForScope(configuration);

            // Web API routes
            configuration.MapHttpAttributeRoutes();
        }
    }
}
