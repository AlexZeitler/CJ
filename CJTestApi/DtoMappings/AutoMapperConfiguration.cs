using AutoMapper;
using AutoMapper.Mappers;
using Castle.Windsor;
using CJ.Mappings;
using CJTestApi.Dtos;

namespace CJTestApi.DtoMappings
{
	public class AutoMapperConfiguration
	{
		public static MappingEngine Configure(IWindsorContainer container)
		{
			var configurationStore = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
			configurationStore.ConstructServicesUsing(container.Resolve);
			var engine = new MappingEngine(configurationStore);
			configurationStore.AddProfile(new GenericDtoToCollectionJsonMappingProfile<BeerDto>(engine));
			
			
			return engine;

		}
	}
}