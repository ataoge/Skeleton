using System;
using System.Collections.Generic;

namespace Ataoge.ObjectMapping
{
    public interface IMapperContext
    {

        IDictionary<string, object> Items { get; }
    }
}