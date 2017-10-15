using System;
using System.Collections.Generic;

namespace Ataoge.ObjectMapping
{
    class AtaogeMapperContext : IMapperContext
    {
        public AtaogeMapperContext()
        {

        }

        
        public IDictionary<string, object> Items {get; set;}
    }
}