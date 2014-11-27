using System.Web.Http;
using CJ.CollectionJson;
using CJTestApi.DtoMappings;
using CJTestApi.Dtos;
using CJTestApi.IoC;
using Owin;

namespace CJTestApi
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();

			var container = ServiceRegistration.RegisterServices(config);
			var engine = AutoMapperConfiguration.Configure(container);

			config.Formatters.Add(new CollectionJsonMediaTypeFormatter<BeerDto>(new CollectionSettings<BeerDto>("1.0"),engine));
			config.MapHttpAttributeRoutes();
			config.EnsureInitialized();
			app.UseWebApi(config);
		}
	}
}