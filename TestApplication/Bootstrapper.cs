using DbDataAccess;
using LightDriverService;
using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using TestApplication.Controllers;
using TestApplication.PrismExtensions;
using TestApplication.Repositories;

namespace TestApplication
{
    public class Bootstrapper
    {
        protected IUnityContainer Container { get; private set; }

        public Bootstrapper(IUnityContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Implement this method to register any required dependencies both repositories and controllers for this API
        /// </summary>
        /// <param name="container"></param>
        protected void ConfigureContainer(IUnityContainer container)
        {
            RegisterType<ILightDriverService, LightDriverService.LightDriverService>();
            RegisterType<IDataService, DataService>();

            RegisterType<IStreetlightRepository, StreetlightRepository>();

            RegisterHttpControllerType<StreetlightController>();

            RegisterControllerType<HomeController>();
        }

        /// <summary>
        /// Retrieves the IoCContainer for this API that contains the capabilities to resolve controllers, repositories etc
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public void SetupContainerForScope(HttpConfiguration configuration)
        {
            ConfigureContainer(Container);
            configuration.DependencyResolver = new InjectionContainer(Container);
        }

        /// <summary>
        /// Registers a new controller type with the container
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        public void RegisterHttpControllerType<TController>()
            where TController : IHttpController
        {
            Container.RegisterType<TController>();
        }

        /// <summary>
        /// Registers a repository interface/type with the container
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RegisterType<TInterface, TConcrete>()
            where TConcrete : TInterface
        {
            Container.RegisterType<TInterface, TConcrete>(new HierarchicalLifetimeManager());
        }

        /// <summary>
        /// Registers a new controller type with the container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RegisterControllerType<T>()
            where T : IController
        {
            Container.RegisterType<T>();
        }
    }
}