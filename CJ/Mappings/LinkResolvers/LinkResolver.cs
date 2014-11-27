using System.Collections.Generic;
using AutoMapper;
using Castle.Core.Internal;
using CollectionJson;

namespace CJ.Mappings.LinkResolvers
{
	public class LinkResolver<T> : ValueResolver<T, List<Link>> {
		private readonly IEnumerable<ILinkResolver<T>> _resolvers;

		public LinkResolver(IEnumerable<ILinkResolver<T>> resolvers) {
			_resolvers = resolvers;
		}

		protected override List<Link> ResolveCore(T source)
		{
			var links = new List<Link>();
			_resolvers.ForEach(resolver => links.AddRange(resolver.ResolveFrom(source)));
			return links;
		}
	}
}