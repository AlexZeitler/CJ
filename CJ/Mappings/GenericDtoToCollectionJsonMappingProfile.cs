using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CJ.Mappings.LinkResolvers;
using CollectionJson;

namespace CJ.Mappings
{
	public class GenericDtoToCollectionJsonMappingProfile<T> : Profile where T : class
	{
		readonly MappingEngine _engine;

		public GenericDtoToCollectionJsonMappingProfile(MappingEngine engine)
		{
			_engine = engine;
		}

		protected override void Configure()
		{
			// T == IDto

			var store = _engine.ConfigurationProvider as ConfigurationStore;

			store.CreateMap<T, List<Data>>().ConstructUsing(MapDtoToListOfData<T>);
			
			

			store.CreateMap<T, Item>()
				// TODO: Self link is Href of Item
				//.ForMember(c=>c.Href,mapper=>mapper.ResolveUsing(arg => throw new NotImplementedException();))
				.ForMember(c => c.Data, mapper => mapper.ResolveUsing(_engine.Map<T, List<Data>>))
				.ForMember(c => c.Links, mapper => mapper.ResolveUsing<LinkResolver<T>>()).AfterMap((dto, item) =>
		        {
		            item.Href = item.Links.FirstOrDefault(l => l.Rel == "Self").Href;
		            item.Links.Remove(item.Links.FirstOrDefault(l => l.Rel == "Self"));
		        });
		        




			store.CreateMap<T, Collection>()
				.ForMember(c => c.Items, mapper => mapper.ResolveUsing((T dto) =>
				{
					var items = new List<Item>
					{
						_engine.Map<T,Item>(dto)
					};
					return items;
				}));
				// TODO: should be collection link resolver -> idea: query all controllers (cached)
				//.ForMember(c => c.Links, mapper => mapper.ResolveUsing<LinkResolver<T>>());



			store.CreateMap<IEnumerable<T>, Collection>()
				.ForMember(c => c.Items, mapper => mapper.ResolveUsing(enumerable => enumerable.Select(_engine.Map<T, Item>)));
		}

	
		List<Data> MapDtoToListOfData<TDto>(ResolutionContext resolutionContext) where TDto : class
		{
			var properties = resolutionContext.SourceType.GetProperties();

			var datas = properties.Select(p =>
			{
				var beerDto = resolutionContext.SourceValue as TDto;
				var value = p.GetValue(beerDto);
				return new Data()
				{
					Name = p.Name.ToLower(),
					Value = value != null ? value.ToString() : null,
					Prompt = p.Name
				};
			});

			return datas.ToList();
		}
	}
}