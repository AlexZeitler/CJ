using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CJ.CollectionJson;
using CJ.Mappings.LinkResolvers;
using CJTestApi.BeerDtoLinkResolvers;
using CJTestApi.DtoMappings;
using CollectionJson;
using CJ.Mappings;
using CJTestApi;
using CJTestApi.Dtos;
using Machine.Specifications;
using Newtonsoft.Json;

namespace CJ.Tests.MappingTests
{
	public class Given_a_beer_dto_when_mapping_it_to_collection_json
	{
		Establish context = () =>
		{
			var container = new WindsorContainer();

			var beerDtoLinkResolver = new LinkResolver<BeerDto>(new List<ILinkResolver<BeerDto>>
			{
				new BeerDtoSelfLinkResolver(new Uri("http://localhost:9002"))
			});

			_beerRequest = new HttpRequestMessage()
			{
				RequestUri = new Uri("http://localhost:9002/api/beer/1")
			};

			container.Register(Component.For<LinkResolver<BeerDto>>().Instance(beerDtoLinkResolver));
			var engine = AutoMapperConfiguration.Configure(container);

			_formatter = new CollectionJsonMediaTypeFormatter<BeerDto>(new CollectionSettings<BeerDto>("1.0"),engine);
			_beerDto = new BeerDto()
			{
				Id = 1,
				Name = "Hopwired IPA"
			};

			var beerDto2 = new BeerDto()
			{
				Id = 2,
				Name = "Tall Poppy"
			};

			_beerDtos = new List<BeerDto>() {_beerDto, beerDto2};

			_jsonSerializer = JsonSerializer.Create();
			
		};

		Because of = () =>
		{
			var ms = new MemoryStream();
			_formatter.GetPerRequestFormatterInstance(_beerDto.GetType(), _beerRequest,
				new MediaTypeHeaderValue("application/vnd.collection+json"));
			var task = _formatter.WriteToStreamAsync(typeof (BeerDto), _beerDto, ms, new FakeContent(), new FakeTransport());
			task.Wait();
			ms.Seek(0, SeekOrigin.Begin);

			_result = _jsonSerializer.Deserialize<ReadDocument>(new JsonTextReader(new StreamReader(ms)));
		};

		It should_map_the_collection_document = () => _result.ShouldNotBeNull();

		It should_map_the_collection = () => _result.Collection.ShouldNotBeNull();

		It should_map_the_collection_version = () => _result.Collection.Version.ShouldEqual("1.0");

		It should_map_1_item = () => _result.Collection.Items.Count.ShouldEqual(1);

		It should_map_lowercase_name_of_properties_to_name_of_item_data_name =
			() => _result.Collection.Items.First().Data.First().Name.ShouldEqual("id");

		It should_map_value_of_properties_to_value_of_item_data_name =
			() => _result.Collection.Items.First().Data.First().Value.ShouldEqual("1");

		It should_map_name_of_properties_to_prompt_of_item_data_name =
			() => _result.Collection.Items.First().Data.First().Prompt.ShouldEqual("Id");

		It should_map_self_link_to_item_href = () => _result.Collection.Items.First()
            .Href.ToString()
            .ShouldEqual("http://localhost:9002/api/beer/1");

		It should_map_collection_self_link = () => _result.Collection.Href.ToString()
			.ShouldEqual("http://localhost:9002/api/beer/1");

		static CollectionJsonMediaTypeFormatter<BeerDto> _formatter;
		static BeerDto _beerDto;
		static List<BeerDto> _beerDtos;
		static JsonSerializer _jsonSerializer;
		static ReadDocument _result;
		static HttpRequestMessage _beerRequest;
	}
}