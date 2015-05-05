using System.Web.Http;
using App.Plugins;
using AutoMapper;
using Microsoft.Owin;
using Owin;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: OwinStartup(typeof(App.Startup))]

namespace App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Configure AutoMapper (http://automapper.codeplex.com/)
            Mapper.Initialize(ConfigureMapper);

            //Configure Bearer Authentication
            ConfigureAuth(app);

            //Log trafic using Log4Net
            app.Use(typeof(Logging));

            //Configure SignalR self host
            ConfigureSignalR(app);

            var config = new HttpConfiguration();

            //Configure AutoFac (http://autofac.org/) for DependencyResolver
            //For more information visit http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
            ConfigureComposition(config);

            //Configure WebApi
            ConfigureWebApi(config);
            app.UseWebApi(config);

         //   var eng = new App.Models.UciEngineManager();
           // eng.TestEngine();
        }
        
    }
}