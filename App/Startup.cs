using System.Web.Http;
using App.Plugins;
using AutoMapper;
using Microsoft.Owin;
using Owin;
using Autofac;
using Microsoft.AspNet.SignalR;
using System.Web.Routing;
using App.Game;
using Microsoft.AspNet.SignalR.Hubs;
using Autofac.Core;
using Microsoft.AspNet.SignalR.Infrastructure;

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
              
            //Configure AutoFac (http://autofac.org/) for DependencyResolver
            //For more information visit http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
            IContainer container = RegisterServices();

            //Configure WebApi
            var config = new HttpConfiguration();
            config.DependencyResolver = new App.Common.AutoFacDependencyResolver(container);         
            ConfigureWebApi(config);
            app.UseWebApi(config);

            //Configure SignalR self host   
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.Resolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);           
            //ConfigureSignalR(app, hubConfiguration);
            app.MapSignalR(hubConfiguration);
          
            //GlobalHost.DependencyResolver = hubConfiguration.Resolver;
        }

    }
}