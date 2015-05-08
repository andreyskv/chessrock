using System.Web.Http;
using App.Common;
using App.Identity;
using App.Models;
using Autofac;
using Microsoft.AspNet.SignalR;
using App.Game;
using Autofac.Core;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Autofac.Integration.SignalR;

namespace App
{
    public partial class Startup
    {
        public static IContainer RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TodoItemRepository>()
                .As<IRepository>()
                .As<IAsyncRepository>();

            builder.RegisterAssemblyTypes(typeof(Startup).Assembly)
                .Where(t => t.Name.EndsWith("Controller"))
                .AsSelf();

            //builder.RegisterInstance(new DomanUserLoginProvider("unevol.cu"))
            //    .As<ILoginProvider>()
            //    .SingleInstance();
            
            builder.RegisterType<LocalUserLoginProvider>().As<ILoginProvider>().SingleInstance();
            builder.RegisterType<ChessHub>();

            builder.RegisterType<ChessGameManager>().As<IChessGameManager>().SingleInstance();              
            //builder.RegisterType<ChessGameManager>().WithParameter(ResolvedParameter.ForNamed<IHubConnectionContext<IChessHub>>("Context")).As<IChessGameManager>().SingleInstance();            
            //var clients = GlobalHost.DependencyResolver.Resolve<IConnectionManager>().GetHubContext<ChessHub,IChessHub>().Clients;            
            //builder.Register(c => clients).Named<IHubConnectionContext<IConnectionManager>>("Context");
      
            return builder.Build();
        } 
        

    }
}