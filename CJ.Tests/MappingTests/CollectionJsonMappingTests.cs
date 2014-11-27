using System.Collections.Generic;
using Castle.Windsor;
using CJ.CollectionJson;
using CJTestApi.DtoMappings;
using CJTestApi.Dtos;
using NUnit.Framework;

namespace CJ.Tests.MappingTests
{
	[TestFixture]
	public class CollectionJsonMappingTests
	{
		CollectionJsonMediaTypeFormatter<BeerDto> _formatter;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			var container = new WindsorContainer();
			var engine = AutoMapperConfiguration.Configure(container);
			_formatter = new CollectionJsonMediaTypeFormatter<BeerDto>(new CollectionSettings<BeerDto>("1.0"),engine);
		}

		[Test]
		public void ShouldCanWriteBeerto()
		{
			Assert.That(_formatter.CanWriteType(typeof (BeerDto)), Is.True);
		}

		[Test]
		public void ShouldCanWriteBeerDtoEnumerable()
		{
			Assert.That(_formatter.CanWriteType(typeof (List<BeerDto>)), Is.True);
		}

		[Test]
		public void ShouldNotCanWriteOtherEnumerable()
		{
			Assert.That(_formatter.CanWriteType(typeof (List<string>)), Is.False);
		}
	}
}