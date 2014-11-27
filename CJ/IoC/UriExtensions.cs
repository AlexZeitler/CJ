using System;
using System.Collections.Generic;
using System.Linq;
using CollectionJson;

namespace CJ.IoC
{
	public static class UriExtensions {
		public static Link ToLink(this UriTemplate uriTemplate, Uri baseUri, string rel,
			params KeyValuePair<string, string>[] templateParameters) {
			var uri = uriTemplate.BindByName(baseUri, templateParameters.ToDictionary(x => x.Key, x => x.Value));
			return new Link() {
				Href = uri,
				Rel = rel
			};
		}
	}
}