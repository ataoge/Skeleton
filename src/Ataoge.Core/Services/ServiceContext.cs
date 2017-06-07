using System;
using Microsoft.Extensions.Logging;

namespace Ataoge.Services
{
    public class ServiceContext:ServiceContextBase
    {
        public ServiceContext():base()
        {

        }

        public override string BrowserInfo => throw new NotImplementedException();

        public override string ClientIpAddress => throw new NotImplementedException();

        public override ILogger Logger => throw new NotImplementedException();
    }
}