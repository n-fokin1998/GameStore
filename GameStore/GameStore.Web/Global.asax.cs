using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using GameStore.AutofacRegistrations;
using GameStore.Web.App_Start;
using GameStore.Web.Infrastructure;
using log4net;

namespace GameStore.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            var logger = LogManager.GetLogger("Logger");
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterInstance<IMapper>(mapper);
            builder.RegisterInstance<ILog>(logger);
            builder.RegisterType<FileSystemAccess>().AsSelf();
            var container = GlobalRegistrations.ConfigureContainer(builder).Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            ClientDataTypeModelValidatorProvider.ResourceClassKey = nameof(App_GlobalResources.ValidationMessagesRes);
            DefaultModelBinder.ResourceClassKey = nameof(App_GlobalResources.ValidationMessagesRes);
        }
    }
}