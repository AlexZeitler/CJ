using System;
using System.Net;
using System.Security.Authentication.ExtendedProtection;

namespace CJ.Tests.MappingTests
{
    public class FakeTransport : TransportContext
    {
        public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
        {
            throw new NotImplementedException();
        }
    }
}