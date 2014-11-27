using System.Net.Http.Headers;

namespace CJ.Tests.MappingTests
{
	internal static class FakeFactory
	{
		internal static FakeContent CreateFakeContent()
		{
			var fakeContent = new FakeContent();
			fakeContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.collection+json");
			return fakeContent;
		}
	}
}