﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CJ.Tests.MappingTests
{
	public class FakeContent : HttpContent
	{
		public FakeContent()
			: base()
		{
		}

		protected override Task SerializeToStreamAsync(Stream stream, TransportContext
			context)
		{
			throw new NotImplementedException();
		}

		protected override bool TryComputeLength(out long length)
		{
			throw new NotImplementedException();
		}
	}
}