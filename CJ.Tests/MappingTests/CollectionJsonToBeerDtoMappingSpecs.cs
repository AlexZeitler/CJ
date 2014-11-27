using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using AutoMapper.Mappers;
using CJ.CollectionJson;
using CJTestApi.Dtos;
using Machine.Specifications;

namespace CJ.Tests.MappingTests
{
	public class Given_a_collection_json_document_when_reading_it
	{
		Establish context = () =>
		{
			var collectionJson =
				"{\"collection\":{\"version\":\"1.0\",\"href\":\"http://localhost:9001/api/beer/1\",\"links\":[],\"items\":[{\"href\":\"http://localhost:9001/api/beer/1\",\"rel\":null,\"rt\":null,\"data\":[{\"name\":\"id\",\"value\":\"1\",\"prompt\":\"Id\"},{\"name\":\"name\",\"value\":\"Hopwired IPA\",\"prompt\":\"Name\"},{\"name\":\"beerstyle\",\"value\":null,\"prompt\":\"BeerStyle\"},{\"name\":\"brewery\",\"value\":null,\"prompt\":\"Brewery\"}],\"links\":[]}],\"queries\":[],\"template\":{\"Data\":[]}}}";


			var configurationStore = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
			var engine = new MappingEngine(configurationStore);

			_formatter = new CollectionJsonMediaTypeFormatter<BeerDto>(new CollectionSettings<BeerDto>("1.0"), engine);
			_memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(collectionJson));
			_content = new StreamContent(_memoryStream);
			_content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.collection+json");
		};


		Because of =
			() =>
			{
				_result =
					_formatter.ReadFromStreamAsync(typeof (TypedReadDocument<BeerDto>), _memoryStream, _content, null).Result as
						TypedReadDocument<BeerDto>;
			};

		It should_map_data_properties = () =>
		{
			_result.ShouldNotBeNull();
		};

		It should_map_item_href = () => {
			_result.Collection.Items.First().Href.ToString().ShouldEqual("http://localhost:9001/api/beer/1");
		};

		static CollectionJsonMediaTypeFormatter<BeerDto> _formatter;
		static HttpContent _content;
		static TypedReadDocument<BeerDto> _result;
		static MemoryStream _memoryStream;
	}
}