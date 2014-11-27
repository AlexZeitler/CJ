using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CJ.IoC
{
    public class WindsorDepdendenciesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
  
            // register all controllers
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IHttpController>()
                .ConfigureFor<ApiController>(c => c.Properties(pi => false))
                .LifestyleTransient());
        }
    }
}