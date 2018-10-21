using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace TestApplication.PrismExtensions
{
    public class ScopeContainer : IDependencyScope
    {
        private IUnityContainer _container;
        protected IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        public ScopeContainer(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "Cannot use a null container when initialising the scope container");
            }
            this._container = container;
        }

        public object GetService(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
            {
                return _container.Resolve(serviceType);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
            {
                return _container.ResolveAll(serviceType);
            }
            else
            {
                return new List<object>();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container.Dispose();
            }
        }

        ~ScopeContainer()
        {
            Dispose(false);
        }

        #endregion
    }
}