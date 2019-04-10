using System;

namespace Ataoge.EventBus.EfCore
{
    public class EfCoreOptions
    {
        internal Type DbContextType { get; set; }

        /// <summary>
        /// Data version
        /// </summary>
        internal string Version { get; set; }

    }
}