using System.Net;
using System.Net.Http;
using System.Web.Http;
using CJTestApi.Dtos;

namespace CJTestApi.Api
{
	public class BeerController : ApiController
	{
		[Route("api/beer/{beerId}")]
		public HttpResponseMessage Get(string beerId)
		{
			var beer = new BeerDto()
			{
				Id = 1,
				Name = "Hopwired IPA"
			};
			return Request.CreateResponse(HttpStatusCode.OK, beer);
		}
	}
}