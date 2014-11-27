using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using CollectionJson;

namespace CJ.CollectionJson
{
	public class CollectionJsonMediaTypeFormatter<T> : JsonMediaTypeFormatter where T : class
	{
		readonly ICollectionSettings<T> _collectionSettings;
		readonly MappingEngine _mappingEngine;
		HttpRequestMessage Request { get; set; }

		public CollectionJsonMediaTypeFormatter(ICollectionSettings<T> collectionSettings, MappingEngine mappingEngine)
		{
			_collectionSettings = collectionSettings;
			_mappingEngine = mappingEngine;
			SupportedMediaTypes.Clear();
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.collection+json"));
		}

		public override bool CanReadType(Type type)
		{
			return (typeof (TypedReadDocument<T>) == type);
		}

		public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
			IFormatterLogger formatterLogger)
		{
			var mapper = _mappingEngine.ConfigurationProvider as ConfigurationStore;
			mapper.CreateMap<IList<Data>, T>().ConvertUsing(list =>
			{
				var instance = Activator.CreateInstance<T>();
				

				var properties = typeof (T).GetProperties();
				foreach (var property in properties)
				{
					var data = list.First(d => String.Equals(d.Name, property.Name, StringComparison.InvariantCultureIgnoreCase));
				    var type1 = property.PropertyType;
				    if (type1 == typeof (Int64))
				    {
                        property.SetValue(instance, int.Parse(data.Value));
					}
					else if (type1 == typeof(Int32)) {
						property.SetValue(instance, int.Parse(data.Value));
					}
				    else
				    {
                        property.SetValue(instance, data.Value);
				    }
					
				}
				return instance;
			});

			var taskSource = new TaskCompletionSource<object>();
			try
			{
				var result =
					base.ReadFromStreamAsync(typeof (ReadDocument), readStream, content, formatterLogger).Result as ReadDocument;

				if (typeof (TypedReadDocument<T>) == type)
				{
					var document = new TypedReadDocument<T>();
					document.Collection.Links = result.Collection.Links;
					document.Collection.Items =
						result.Collection.Items.Select(
							item => { return (new Item<T>() {Data = _mappingEngine.Map<IList<Data>, T>(item.Data), Href = item.Href, Links = item.Links}); })
							.ToList();
					taskSource.SetResult(document);
				}
			}
			catch (Exception e)
			{
				taskSource.SetException(e);
			}
			return taskSource.Task;
		}


		public override bool CanWriteType(Type type)
		{
			return (typeof (T) == type || type.IsGenericType && typeof (T) == (type.GenericTypeArguments[0]));
		}

		public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
			TransportContext transportContext)
		{
			var readDocument = new ReadDocument();


			if ((typeof (T) == (type)))
				readDocument.Collection = _mappingEngine.Map<T, Collection>(value as T);


			if ((type.IsGenericType && typeof (T).IsAssignableFrom(type.GenericTypeArguments[0])))
				readDocument.Collection = _mappingEngine.Map<IEnumerable<T>, Collection>(value as IEnumerable<T>);

			readDocument.Collection.Href = Request.RequestUri;
			readDocument.Collection.Version = _collectionSettings.Version;
			value = readDocument;
			return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
		}

		public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request,
			MediaTypeHeaderValue mediaType)
		{
			Request = request;
			return base.GetPerRequestFormatterInstance(type, request, mediaType);
		}
	}

	public interface ICollectionSettings<T>
	{
		string Version { get; }
	}

	public class CollectionSettings<T> : ICollectionSettings<T>
	{
		public CollectionSettings(string version)
		{
			Version = version;
		}

		public string Version { get; private set; }
	}
}