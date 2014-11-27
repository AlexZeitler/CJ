using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.Proxy;
using CJ.IoC;

namespace Secur.IMS.SelfHost.IoC
{
    public class ServiceRegistration
    {
        public static void RegisterServices(HttpConfiguration configuration)
        {
            var defaultKernel = new DefaultKernel(
                new InlineDependenciesPropagatingDependencyResolver(),
                new DefaultProxyFactory());

            // disable property injection
            var propertyInspector = defaultKernel.ComponentModelBuilder
                .Contributors
                .OfType<PropertiesDependenciesModelInspector>()
                .Single();
            defaultKernel.ComponentModelBuilder.RemoveContributor(propertyInspector);


            // create instance with replaced controller resolver
            var container =
                new WindsorContainer(
                    defaultKernel,
                    new DefaultComponentInstaller())
                    .Install(new WindsorDepdendenciesInstaller());

            configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorControllerActivator(container));
        }
    }
}