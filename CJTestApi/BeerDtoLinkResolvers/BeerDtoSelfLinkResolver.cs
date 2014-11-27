using System;
using System.Collections.Generic;
using CJ.IoC;
using CJ.Mappings.LinkResolvers;
using CJTestApi.Dtos;
using CollectionJson;

namespace CJTestApi.BeerDtoLinkResolvers
{
	public class BeerDtoSelfLinkResolver : ILinkResolver<BeerDto> {
		private readonly Uri _baseUri;

		public BeerDtoSelfLinkResolver(Uri baseUri) {
			_baseUri = baseUri;
		}

		public IEnumerable<Link> ResolveFrom(BeerDto entity) {
			var selfLinkTemplate = new UriTemplate("api/beer/{beerId}");

			var selfLink =
				selfLinkTemplate.ToLink(_baseUri, "Self",
					new KeyValuePair<string, string>("beerId", entity.Id.ToString()));

			return new Link[] { selfLink };
		}
	}
}