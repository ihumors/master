using LazyCache;
using System.Configuration;
using Autofac;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using DbDependencies = Dms.Repository.Dependencies;
using Dms.Service.Interfaces;
using Dms.Service.Implements;
using Dms.Repository.Implements;
using Dms.Repository.Interfaces;
using System;
using System.Reflection;
using Dms.Repository;
using Dms.Service;

namespace Dms.Master.App_Start
{
    public class Dependencies
    {
        public static readonly IAppCache cache = new CachingService();
        public static IContainer container;

        public static void RegisterServices()
        {
            var builder = new ContainerBuilder();
            
            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            // Register all module dependencies
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());

            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void RegisterConnectionStrings()
        {
            DbDependencies.LotteryWriterConnection = ConnectionStrings("LotteryWriterConnection");
        }

        public static void RegisterAppConfigs()
        {
        }

        private static string ConnectionStrings(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        private static string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}