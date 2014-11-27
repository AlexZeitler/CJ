using System;
using System.Collections.Generic;
using CollectionJson;

namespace CJ.CollectionJson
{
	public class TypedReadDocument<T>
	{
		public TypedReadDocument()
		{
			Collection = new Collection<T>();
		}

		public Collection<T> Collection { get; set; }
	}

	public class Collection<T>
	{
		public Collection()
		{
			Items = new List<Item<T>>();
		}
		public IList<Item<T>> Items { get; set; }
		public IList<Link> Links { get; set; }
	}

	public class Item<T>
	{
		public T Data { get; set; }
		public Uri Href { get; set; }
		public IList<Link> Links { get; set; } 
	}
}