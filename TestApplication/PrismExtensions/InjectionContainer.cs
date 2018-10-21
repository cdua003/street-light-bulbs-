using Microsoft.Practices.Unity;
using System.Web.Http.Dependencies;

namespace TestApplication.PrismExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class InjectionContainer : ScopeContainer, IDependencyResolver
    {
        public InjectionContainer(IUnityContainer container)
            : base(container)
        {

        }

        public IDependencyScope BeginScope()
        {
            var child = this.Container.CreateChildContainer();
            return new ScopeContainer(child);
        }
    }
}