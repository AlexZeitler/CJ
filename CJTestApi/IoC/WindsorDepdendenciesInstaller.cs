using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CJ.Mappings.LinkResolvers;
using CJTestApi.BeerDtoLinkResolvers;
using CJTestApi.Dtos;

namespace CJTestApi.IoC
{
    public class WindsorDepdendenciesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
			var beerDtoLinkResolver = new LinkResolver<BeerDto>(new List<ILinkResolver<BeerDto>>
			{
				new BeerDtoSelfLinkResolver(new Uri("http://localhost:9001")) 
			});

			container.Register(Component.For<LinkResolver<BeerDto>>().Instance(beerDtoLinkResolver));


            // register all controllers
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IHttpController>()
                .ConfigureFor<ApiController>(c => c.Properties(pi => false))
                .LifestyleTransient());
        }
    }
}