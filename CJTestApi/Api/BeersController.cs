using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CJTestApi.Dtos;

namespace CJTestApi.Api
{
	public class BeersController : ApiController
	{
		[Route("api/beers")]
		public HttpResponseMessage Get()
		{
			var beers = new List<BeerDto>()
			{
				new BeerDto()
				{
					Id = 1,
					Name = "Hopwired IPA"
				},
				new BeerDto()
				{
					Id = 2,
					Name = "Tall Poppy"
				}
			};
			return Request.CreateResponse(HttpStatusCode.OK, beers);
		}
	}
}