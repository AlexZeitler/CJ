using System.Collections.Generic;
using CollectionJson;

namespace CJ.Mappings.LinkResolvers
{
	public interface ILinkResolver<T> {
		IEnumerable<Link> ResolveFrom(T entity);
	}
}